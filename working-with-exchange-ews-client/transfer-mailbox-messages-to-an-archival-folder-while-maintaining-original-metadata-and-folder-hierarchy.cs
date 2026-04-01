using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder connection settings – replace with real values.
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against running with placeholder credentials.
            if (string.IsNullOrWhiteSpace(serviceUrl) || serviceUrl.Contains("example"))
            {
                Console.WriteLine("Please provide valid Exchange service URL and credentials.");
                return;
            }

            // Create the EWS client using the factory method.
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, new NetworkCredential(username, password)))
            {
                // Source folder – here we use the Inbox. Adjust as needed.
                string sourceFolderUri = client.MailboxInfo.InboxUri;

                // List all messages in the source folder.
                ExchangeMessageInfoCollection messages = client.ListMessages(sourceFolderUri);

                // Archive each message while preserving its metadata.
                foreach (ExchangeMessageInfo msgInfo in messages)
                {
                    // ArchiveItem moves the item to the user's archive mailbox.
                    client.ArchiveItem(sourceFolderUri, msgInfo.UniqueUri);
                }

                Console.WriteLine($"Archived {messages.Count} messages from the Inbox.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
