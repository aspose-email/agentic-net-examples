using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip real network call when placeholders are detected
            if (mailboxUri.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping Exchange operations.");
                return;
            }

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Get mailbox information
                ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();
                Console.WriteLine($"Inbox URI: {mailboxInfo.InboxUri}");

                // List messages in the Inbox
                ExchangeMessageInfoCollection messages = client.ListMessages(mailboxInfo.InboxUri);
                Console.WriteLine($"Found {messages.Count} messages in Inbox.");

                if (messages.Count > 0)
                {
                    // Fetch the first message
                    string firstMessageUri = messages[0].UniqueUri;
                    using (MailMessage message = client.FetchMessage(firstMessageUri))
                    {
                        Console.WriteLine($"Subject: {message.Subject}");

                        // Save the message to a local file (guarded file I/O)
                        string outputPath = "message.eml";
                        string outputDir = Path.GetDirectoryName(outputPath);
                        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                        {
                            Directory.CreateDirectory(outputDir);
                        }

                        try
                        {
                            message.Save(outputPath, SaveOptions.DefaultEml);
                            Console.WriteLine($"Message saved to {outputPath}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
