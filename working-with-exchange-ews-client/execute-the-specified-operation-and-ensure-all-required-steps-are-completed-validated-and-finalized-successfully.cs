using Aspose.Email.Clients.Exchange;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – skip actual network call in CI environments
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            if (mailboxUri.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected – skipping EWS operations.");
                return;
            }

            // Create the EWS client using the factory method
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // List messages in the Inbox folder
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);
                    if (messages == null || messages.Count == 0)
                    {
                        Console.WriteLine("No messages found in the Inbox.");
                        return;
                    }

                    // Fetch the first message
                    string messageUri = messages[0].UniqueUri;
                    using (MailMessage message = client.FetchMessage(messageUri))
                    {
                        // Prepare output path
                        string outputPath = Path.Combine(Environment.CurrentDirectory, "output.eml");
                        string outputDir = Path.GetDirectoryName(outputPath);
                        if (!Directory.Exists(outputDir))
                        {
                            Directory.CreateDirectory(outputDir);
                        }

                        // Save the message to a file
                        try
                        {
                            message.Save(outputPath, SaveOptions.DefaultEml);
                            Console.WriteLine($"Message saved to: {outputPath}");
                        }
                        catch (Exception ioEx)
                        {
                            Console.Error.WriteLine($"Failed to save message: {ioEx.Message}");
                        }
                    }
                }
                catch (Exception clientEx)
                {
                    Console.Error.WriteLine($"EWS operation failed: {clientEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
