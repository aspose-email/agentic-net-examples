using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email;
using System;
using System.IO;
using System.Net;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Server connection parameters
            string exchangeServerUri = "http://exchange.example.com/Exchange";
            string userName = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (exchangeServerUri.Contains("example.com") || userName.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create credentials
            NetworkCredential credentials = new NetworkCredential(userName, password);

            // Initialize Exchange client
            using (IEWSClient client = EWSClient.GetEWSClient(exchangeServerUri, credentials))
            {
                // Get the Inbox folder URI
                string inboxUri = client.MailboxInfo.InboxUri;

                // List messages in the Inbox
                ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri);
                if (messages == null || messages.Count == 0)
                {
                    Console.Error.WriteLine("No messages found in the Inbox.");
                    return;
                }

                // Select the first message (or any desired message)
                ExchangeMessageInfo messageInfo = messages[0];
                string messageUri = messageInfo.UniqueUri;

                // Define output path for the EML file
                string outputDirectory = Path.Combine(Environment.CurrentDirectory, "ExportedEmails");
                string outputPath = Path.Combine(outputDirectory, "ExportedMessage.eml");

                // Ensure the output directory exists
                try
                {
                    if (!Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }

                // Save the message as EML preserving MIME headers and attachments
                try
                {
                    client.SaveMessage(messageUri, outputPath);
                    Console.WriteLine($"Message saved successfully to: {outputPath}");
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Failed to save message: {saveEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
