using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string distListPath = "distributionList.msg";

            // Ensure the file exists; create a minimal placeholder if missing.
            if (!File.Exists(distListPath))
            {
                try
                {
                    using (MapiDistributionList placeholder = new MapiDistributionList())
                    {
                        placeholder.DisplayName = "Sample Distribution List";
                        placeholder.Members.Add(new MapiDistributionListMember("John Doe", "john.doe@example.com"));
                        placeholder.Save(distListPath);
                        Console.WriteLine($"Placeholder distribution list created at '{distListPath}'.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder distribution list: {ex.Message}");
                    return;
                }
            }

            // Load the existing distribution list message.
            try
            {
                using (MapiMessage msg = MapiMessage.Load(distListPath))
                {
                    if (msg.SupportedType != MapiItemType.DistList)
                    {
                        Console.Error.WriteLine("The specified file is not a distribution list.");
                        return;
                    }

                    using (MapiDistributionList distList = (MapiDistributionList)msg.ToMapiMessageItem())
                    {
                        // Merge duplicate members based on email (case‑insensitive).
                        Dictionary<string, MapiDistributionListMember> uniqueMembers = new Dictionary<string, MapiDistributionListMember>(StringComparer.OrdinalIgnoreCase);
                        foreach (MapiDistributionListMember member in distList.Members)
                        {
                            string email = member.EmailAddress?.Trim() ?? string.Empty;
                            if (string.IsNullOrEmpty(email))
                                continue;

                            if (!uniqueMembers.ContainsKey(email))
                            {
                                // First occurrence – keep as‑is.
                                uniqueMembers[email] = new MapiDistributionListMember(member.DisplayName, email);
                            }
                            else
                            {
                                // Duplicate – optionally merge display names (keep the first one).
                                // Additional merging logic could be added here.
                            }
                        }

                        // Replace the members collection with the merged set.
                        distList.Members.Clear();
                        foreach (MapiDistributionListMember mergedMember in uniqueMembers.Values)
                        {
                            distList.Members.Add(mergedMember);
                        }

                        // Save the updated distribution list back to the file.
                        try
                        {
                            distList.Save(distListPath);
                            Console.WriteLine("Duplicate distribution list entries merged successfully.");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save updated distribution list: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing distribution list file: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
