using Aspose.Email.Clients.Exchange;
using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Exchange.Dav;

// Author: Aspose.Email sample
class Program
{
    static void Main()
    {
        try
        {
            // Connection parameters (replace with real values)
            string mailboxUri = "https://exchange.example.com/ews/exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create and dispose the Exchange client
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                try
                {
                    // Validate credentials by listing messages in the Inbox folder
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);
                    if (messages == null || messages.Count == 0)
                    {
                        Console.WriteLine("No messages found in the Inbox folder.");
                        return;
                    }

                    // Fetch the first message
                    ExchangeMessageInfo firstInfo = messages[0];
                    using (MailMessage message = client.FetchMessage(firstInfo.UniqueUri))
                    {
                        // Prepare output directory and file path
                        string outputDir = "Output";

                        // Skip external calls when placeholder credentials are used
                        if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
                        {
                            Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                            return;
                        }

                        string outputPath = Path.Combine(outputDir, "FirstMessage.msg");

                        if (!Directory.Exists(outputDir))
                        {
                            Console.Error.WriteLine($"Directory does not exist: {outputDir}");
                            return;
                        }

                        try
                        {
                            // Save the message to a .msg file
                            message.Save(outputPath);
                            Console.WriteLine($"Message saved to {outputPath}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Error saving message: {ex.Message}");
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Exchange operation failed: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
