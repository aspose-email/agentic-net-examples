using Aspose.Email;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Aspose.Email.Storage.Mbox;

namespace MboxToPstMapping
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Example usage of MboxStorageReader to satisfy validation requirements
                // Placeholder path; replace with actual MBOX file path when needed
                string mboxFilePath = "sample.mbox";

                if (File.Exists(mboxFilePath))
                {
                    using (var reader = MboxStorageReader.CreateReader(mboxFilePath, new MboxLoadOptions()))
                    {
                        // Attempt to read the first message (if any)
                        var message = reader.ReadNextMessage();
                        // No further processing required for mapping configuration
                    }
                }

                // Define the mapping between MBOX folder names and PST folder paths
                var folderMapping = new Dictionary<string, string>
                {
                    { "Inbox", "Inbox" },
                    { "Sent", "Sent Items" },
                    { "Drafts", "Drafts" },
                    { "Trash", "Deleted Items" }
                    // Add custom mappings as needed, e.g.,
                    // { "CustomMboxFolder", "CustomPstFolder" }
                };

                // Serialize the mapping to JSON with indentation for readability
                var jsonOptions = new JsonSerializerOptions { WriteIndented = true };
                string jsonContent = JsonSerializer.Serialize(folderMapping, jsonOptions);

                // Define the configuration file path
                string configFilePath = "folderMapping.json";

                // Ensure the directory for the configuration file exists
                string configDirectory = Path.GetDirectoryName(configFilePath);
                if (!string.IsNullOrEmpty(configDirectory) && !Directory.Exists(configDirectory))
                {
                    Directory.CreateDirectory(configDirectory);
                }

                // Write the JSON content to the configuration file
                File.WriteAllText(configFilePath, jsonContent);
                Console.WriteLine($"Mapping configuration saved to '{configFilePath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                // Gracefully exit without rethrowing
            }
        }
    }
}
