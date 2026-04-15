using Aspose.Email;
using System;
using System.IO;
using System.Text.Json;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Path for the backup JSON file
            string outputPath = "backup.json";

            // Ensure the output directory exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create a sample MAPI message and set follow‑up options
            using (MapiMessage message = new MapiMessage("from@example.com", "to@example.com", "Sample Subject", "Sample body"))
            {
                // Configure follow‑up options including voting buttons
                FollowUpOptions options = new FollowUpOptions();
                options.FlagRequest = "Please review";
                options.DueDate = DateTime.Now.AddDays(2);
                options.StartDate = DateTime.Now;
                options.ReminderTime = DateTime.Now.AddDays(1);
                options.VotingButtons = "Approve;Reject";
                options.Categories = "Finance;Urgent";

                // Apply the options to the message
                FollowUpManager.SetOptions(message, options);

                // Retrieve the options back from the message
                FollowUpOptions retrieved = FollowUpManager.GetOptions(message);

                // Prepare an object that holds the voting and follow‑up configuration
                var backupConfig = new
                {
                    VotingButtons = retrieved.VotingButtons,
                    FlagRequest = retrieved.FlagRequest,
                    DueDate = retrieved.DueDate,
                    StartDate = retrieved.StartDate,
                    ReminderTime = retrieved.ReminderTime,
                    Categories = retrieved.Categories
                };

                // Serialize the configuration to JSON
                string json = JsonSerializer.Serialize(backupConfig, new JsonSerializerOptions { WriteIndented = true });

                // Write the JSON to the file with error handling
                try
                {
                    File.WriteAllText(outputPath, json);
                    Console.WriteLine($"Backup saved to {outputPath}");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to write backup file: {ioEx.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            return;
        }
    }
}
