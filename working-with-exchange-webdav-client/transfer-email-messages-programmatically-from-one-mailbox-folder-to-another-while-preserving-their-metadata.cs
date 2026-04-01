using Aspose.Email.Clients.Exchange;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;


namespace EmailTransferSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder Exchange server URL and credentials.
                string exchangeUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Guard: skip execution when placeholder credentials are detected.
                if (exchangeUrl.Contains("example.com"))
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping operation.");
                    return;
                }

                // Initialize the Exchange client.
                using (ExchangeClient client = new ExchangeClient(exchangeUrl, username, password))
                {
                    // Define source and destination folder URIs.
                    // Here we use Inbox as source and Drafts as destination for illustration.
                    string sourceFolderUri = client.MailboxInfo.InboxUri;
                    string destinationFolderUri = client.MailboxInfo.DraftsUri;

                    // List messages in the source folder.
                    ExchangeMessageInfoCollection messageInfos = client.ListMessages(sourceFolderUri);

                    // Iterate over each message and move it to the destination folder.
                    foreach (ExchangeMessageInfo messageInfo in messageInfos)
                    {
                        // MoveMessage preserves metadata such as subject, flags, etc.
                        client.MoveMessage(messageInfo, destinationFolderUri);
                        Console.WriteLine($"Moved message: {messageInfo.Subject}");
                    }
                }
            }
            catch (Exception ex)
            {
                // Top‑level exception guard: output error without crashing.
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
