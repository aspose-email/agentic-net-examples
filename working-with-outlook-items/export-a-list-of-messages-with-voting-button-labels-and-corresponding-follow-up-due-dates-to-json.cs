using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for input PST and output JSON
            string pstPath = "messages.pst";
            string jsonOutputPath = "voting_info.json";

            // Verify PST file exists
            if (!File.Exists(pstPath))
            {
                Console.Error.WriteLine($"Input PST file not found: {pstPath}");
                return;
            }

            // Prepare a list to hold export data
            List<object> exportData = new List<object>();

            // Load PST and iterate messages
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Access the Inbox folder (adjust name if needed)
                FolderInfo inboxFolder = pst.RootFolder.GetSubFolder("Inbox");

                foreach (MessageInfo messageInfo in inboxFolder.EnumerateMessages())
                {
                    using (MapiMessage mapiMessage = pst.ExtractMessage(messageInfo))
                    {
                        // Retrieve voting button labels
                        string[] votingButtons = FollowUpManager.GetVotingButtons(mapiMessage);

                        // Retrieve follow‑up options to get due date
                        FollowUpOptions followUpOptions = FollowUpManager.GetOptions(mapiMessage);
                        DateTime? dueDate = followUpOptions?.DueDate;

                        // Build an anonymous object for JSON serialization
                        var entry = new
                        {
                            Subject = mapiMessage.Subject,
                            VotingButtons = votingButtons,
                            DueDate = dueDate?.ToString("o") // ISO 8601 format
                        };

                        exportData.Add(entry);
                    }
                }
            }

            // Serialize the list to JSON
            string json = JsonSerializer.Serialize(exportData, new JsonSerializerOptions { WriteIndented = true });

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(jsonOutputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Write JSON to file with error handling
            try
            {
                File.WriteAllText(jsonOutputPath, json);
                Console.WriteLine($"Export completed successfully. JSON saved to: {jsonOutputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to write JSON file: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
