using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define output file path
            string outputPath = "outputDistributionList.msg";

            // Ensure the directory exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create a new MAPI distribution list
            using (MapiDistributionList distributionList = new MapiDistributionList())
            {
                distributionList.DisplayName = "Sample Distribution List";

                // Prepare members (some duplicates included for demonstration)
                var membersToAdd = new List<MapiDistributionListMember>
                {
                    new MapiDistributionListMember("Alice", "alice@example.com"),
                    new MapiDistributionListMember("Bob", "bob@example.com"),
                    new MapiDistributionListMember("Charlie", "charlie@example.com"),
                    new MapiDistributionListMember("Bob Duplicate", "bob@example.com") // duplicate
                };

                // Collection to hold unique members
                MapiDistributionListMemberCollection uniqueMembers = new MapiDistributionListMemberCollection();
                HashSet<string> emailSet = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

                foreach (var member in membersToAdd)
                {
                    if (emailSet.Add(member.EmailAddress))
                    {
                        uniqueMembers.Add(member);
                    }
                    else
                    {
                        Console.Error.WriteLine($"Duplicate email detected and skipped: {member.EmailAddress}");
                    }
                }

                // Assign the filtered unique members to the distribution list
                distributionList.Members.AddRange(uniqueMembers);

                // Save the distribution list to a file (guarded with try/catch)
                try
                {
                    distributionList.Save(outputPath);
                    Console.WriteLine($"Distribution list saved successfully to '{outputPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save distribution list: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
