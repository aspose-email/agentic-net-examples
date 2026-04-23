using Aspose.Email.Clients.Exchange;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials/hosts
            if (mailboxUri.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create and connect the Exchange client
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // List messages from the Inbox folder
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);

                // Prepare a list for JSON serialization
                List<object> reportItems = new List<object>();

                foreach (ExchangeMessageInfo messageInfo in messages)
                {
                    string subject = messageInfo.Subject;
                    string senderAddress = messageInfo.Sender?.Address ?? string.Empty;
                    long size = messageInfo.Size;

                    reportItems.Add(new
                    {
                        Subject = subject,
                        Sender = senderAddress,
                        Size = size
                    });
                }

                // Serialize the report to JSON
                string jsonReport = JsonSerializer.Serialize(reportItems, new JsonSerializerOptions { WriteIndented = true });

                // Define output file path
                string outputPath = "report.json";

                // Ensure the directory exists
                try
                {
                    string directory = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    // Write JSON to file
                    File.WriteAllText(outputPath, jsonReport);
                    Console.WriteLine($"Report generated at: {outputPath}");
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"File I/O error: {ioEx.Message}");
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
