using Aspose.Email.Clients.Exchange;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Skip execution if placeholder credentials are detected
            if (mailboxUri.Contains("example.com") || username == "username")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create and use the Exchange client
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                try
                {
                    // Retrieve messages from the Inbox folder that match the query.
                    string query = "HasAttachment = True AND IsRead = False";
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, query);
                    foreach (var msgInfo in messages)
                    {
                        // Fetch the full message
                        using (MailMessage message = client.FetchMessage(msgInfo.UniqueUri))
                        {
                            Console.WriteLine($"Subject: {message.Subject}");

                            // Prepare output directory and file path
                            string outputDir = "Output";
                            string outputPath = Path.Combine(outputDir, "FirstMessage.eml");

                            // Ensure the output directory exists
                            if (!Directory.Exists(outputDir))
                            {
                                Directory.CreateDirectory(outputDir);
                            }

                            // Save the message to a file with error handling
                            try
                            {
                                message.Save(outputPath);
                                Console.WriteLine($"Message saved to {outputPath}");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                            }
                        }

                        // Process only the first message
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during Exchange operations: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
