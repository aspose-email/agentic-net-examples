using Aspose.Email.Clients.Exchange;
using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Exchange Web Services endpoint and credentials
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create the EWS client (client variable name must be preserved)
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
            {
                // Get the Inbox folder URI
                string inboxUri = client.MailboxInfo.InboxUri;

                // List messages in the Inbox
                ExchangeMessageInfoCollection messagesInfo = client.ListMessages(inboxUri);
                if (messagesInfo == null || messagesInfo.Count == 0)
                {
                    Console.WriteLine("No messages found in the Inbox.");
                    return;
                }

                // Select the first message
                string messageUri = messagesInfo[0].UniqueUri;

                // Fetch the full message
                using (MailMessage message = client.FetchMessage(messageUri))
                {
                    // Remove extended (custom) headers to streamline storage
                    message.Headers.Clear();

                    // Define output path
                    string outputPath = "cleaned.eml";

                    // Ensure the output directory exists
                    string outputDir = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }

                    // Save the cleaned message
                    message.Save(outputPath, SaveOptions.DefaultEml);
                    Console.WriteLine($"Message saved without extended properties to: {outputPath}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
