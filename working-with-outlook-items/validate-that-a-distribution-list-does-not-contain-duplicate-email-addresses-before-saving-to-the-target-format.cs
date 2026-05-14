using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string inputPath = "distributionList.msg";
            string outputPath = "distributionListValidated.msg";

            // Ensure the input file exists; create a minimal placeholder if it does not.
            if (!File.Exists(inputPath))
            {
                using (MapiDistributionList placeholder = new MapiDistributionList())
                {
                    placeholder.DisplayName = "Placeholder List";
                    placeholder.Save(inputPath);
                }
                Console.WriteLine($"Placeholder distribution list created at '{inputPath}'.");
            }

            // Load the message and verify it is a distribution list.
            using (MapiMessage message = MapiMessage.Load(inputPath))
            {
                if (message.SupportedType != MapiItemType.DistList)
                {
                    Console.WriteLine("The specified file is not a distribution list.");
                    return;
                }

                using (MapiDistributionList distributionList = (MapiDistributionList)message.ToMapiMessageItem())
                {
                    // Validate that there are no duplicate email addresses.
                    HashSet<string> seenEmails = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                    bool hasDuplicate = false;

                    foreach (MapiDistributionListMember member in distributionList.Members)
                    {
                        if (!seenEmails.Add(member.EmailAddress))
                        {
                            Console.WriteLine($"Duplicate email address detected: {member.EmailAddress}");
                            hasDuplicate = true;
                        }
                    }

                    if (hasDuplicate)
                    {
                        Console.WriteLine("Distribution list contains duplicate email addresses. Saving aborted.");
                        return;
                    }

                    // No duplicates found; save the validated distribution list.
                    distributionList.Save(outputPath);
                    Console.WriteLine($"Validated distribution list saved to '{outputPath}'.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
