using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Input MSG file containing the distribution list
            string inputMsgPath = "distributionList.msg";
            // Output CSV file path
            string outputCsvPath = "distributionListMembers.csv";

            // Guard input file existence
            if (!File.Exists(inputMsgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputMsgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {inputMsgPath}");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputCsvPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }
            }

            // Load the MSG file and extract distribution list members
            using (MapiMessage msg = MapiMessage.Load(inputMsgPath))
            {
                if (msg.SupportedType != MapiItemType.DistList)
                {
                    Console.Error.WriteLine("The provided MSG file is not a distribution list.");
                    return;
                }

                // Convert to MapiDistributionList
                MapiDistributionList distList = (MapiDistributionList)msg.ToMapiMessageItem();

                // Prepare CSV writer
                try
                {
                    using (StreamWriter writer = new StreamWriter(outputCsvPath, false))
                    {
                        // Write CSV header
                        writer.WriteLine("Name,Email,Role");

                        // Iterate over members
                        MapiDistributionListMemberCollection members = distList.Members;
                        foreach (MapiDistributionListMember member in members)
                        {
                            // Role information is not available in MapiDistributionListMember; leave empty
                            string name = member.DisplayName ?? string.Empty;
                            string email = member.EmailAddress ?? string.Empty;
                            string role = string.Empty;

                            // Escape commas in fields if necessary
                            string escapedName = name.Contains(",") ? $"\"{name}\"" : name;
                            string escapedEmail = email.Contains(",") ? $"\"{email}\"" : email;
                            string escapedRole = role.Contains(",") ? $"\"{role}\"" : role;

                            writer.WriteLine($"{escapedName},{escapedEmail},{escapedRole}");
                        }
                    }

                    Console.WriteLine($"Distribution list members have been written to: {outputCsvPath}");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Error writing CSV file: {ioEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
