using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the MSG file containing the distribution list
            string msgPath = "distributionList.msg";

            // Verify the MSG file exists
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"File not found: {msgPath}");
                return;
            }

            // Load the MSG file
            MapiMessage msg = MapiMessage.Load(msgPath);

            // Ensure the message is a distribution list
            if (msg.SupportedType != MapiItemType.DistList)
            {
                Console.Error.WriteLine("The provided MSG file is not a distribution list.");
                return;
            }

            // Convert to MapiDistributionList
            MapiDistributionList distributionList = (MapiDistributionList)msg.ToMapiMessageItem();

            // Get the members collection
            MapiDistributionListMemberCollection members = distributionList.Members;

            // Output CSV file path
            string csvPath = "distributionList.csv";

            // Write members to CSV
            using (StreamWriter writer = new StreamWriter(csvPath, false))
            {
                writer.WriteLine("Name,Email,Role");
                foreach (MapiDistributionListMember member in members)
                {
                    string name = member.DisplayName ?? string.Empty;
                    string email = member.EmailAddress ?? string.Empty;
                    // Role information is not available in MAPI distribution list; leave empty
                    string role = string.Empty;

                    // Escape double quotes in fields
                    name = name.Replace("\"", "\"\"");
                    email = email.Replace("\"", "\"\"");
                    role = role.Replace("\"", "\"\"");

                    writer.WriteLine($"\"{name}\",\"{email}\",\"{role}\"");
                }
            }

            Console.WriteLine($"Distribution list members have been written to {csvPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
