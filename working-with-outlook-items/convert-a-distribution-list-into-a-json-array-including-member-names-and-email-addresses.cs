using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the MSG file that contains the distribution list
            string msgPath = "distributionList.msg";

            // Verify that the input file exists
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

                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            // Load the MSG file
            MapiMessage message;
            try
            {
                message = MapiMessage.Load(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            // Ensure the message is a distribution list
            if (message.SupportedType != MapiItemType.DistList)
            {
                Console.Error.WriteLine("The provided MSG file is not a distribution list.");
                return;
            }

            // Convert to MapiDistributionList
            MapiDistributionList distributionList = (MapiDistributionList)message.ToMapiMessageItem();

            // Prepare a list to hold member information
            List<Dictionary<string, string>> membersJson = new List<Dictionary<string, string>>();

            // Iterate over members and collect name and email address
            foreach (MapiDistributionListMember member in distributionList.Members)
            {
                Dictionary<string, string> entry = new Dictionary<string, string>();
                entry["Name"] = member.DisplayName ?? string.Empty;
                entry["Email"] = member.EmailAddress ?? string.Empty;
                membersJson.Add(entry);
            }

            // Serialize the list to JSON
            string jsonOutput = JsonSerializer.Serialize(membersJson, new JsonSerializerOptions { WriteIndented = true });

            // Path for the output JSON file
            string jsonPath = "distributionList.json";

            // Write JSON to file
            try
            {
                File.WriteAllText(jsonPath, jsonOutput);
                Console.WriteLine($"Distribution list exported to JSON file: {jsonPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to write JSON file: {ex.Message}");
            }

            // Dispose the loaded message
            message.Dispose();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
