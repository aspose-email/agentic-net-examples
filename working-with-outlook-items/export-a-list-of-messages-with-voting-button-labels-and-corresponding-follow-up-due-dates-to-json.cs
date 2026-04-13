using System;
using System.IO;
using System.Collections.Generic;
using System.Text.Json;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details
            string mailboxUri = "https://exchange.example.com/ews/exchange.asmx";
            string username = "username";
            string password = "password";

            // Skip execution when placeholders are detected
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Connect to Exchange server
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // List messages in the Inbox folder
                ExchangeMessageInfoCollection messageInfos = client.ListMessages("Inbox");

                // Prepare a collection for JSON serialization
                List<object> reportItems = new List<object>();

                foreach (ExchangeMessageInfo info in messageInfos)
                {
                    // Fetch the message as a MAPI object to access voting buttons and follow‑up options
                    using (MapiMessage mapiMessage = client.FetchMapiMessage(info.UniqueUri))
                    {
                        // Retrieve voting button labels
                        string[] votingButtons = FollowUpManager.GetVotingButtons(mapiMessage);

                        // Retrieve follow‑up options (may contain due date)
                        FollowUpOptions options = FollowUpManager.GetOptions(mapiMessage);
                        DateTime? dueDate = null;
                        if (options != null && options.DueDate != DateTime.MinValue)
                        {
                            dueDate = options.DueDate;
                        }

                        // Create an anonymous object for the current message
                        var item = new
                        {
                            Subject = info.Subject,
                            VotingButtons = votingButtons,
                            DueDate = dueDate
                        };

                        reportItems.Add(item);
                    }
                }

                // Define output file path
                string outputPath = "voting_report.json";
                string outputDirectory = Path.GetDirectoryName(outputPath);

                // Ensure the output directory exists
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Serialize to JSON and write to file
                try
                {
                    string json = JsonSerializer.Serialize(reportItems, new JsonSerializerOptions { WriteIndented = true });
                    File.WriteAllText(outputPath, json);
                    Console.WriteLine($"Report saved to {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to write JSON file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
