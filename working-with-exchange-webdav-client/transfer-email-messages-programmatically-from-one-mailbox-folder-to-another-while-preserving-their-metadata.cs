using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Author note: Adjust the service URL, credentials, and folder URIs as needed for your environment.
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create the Exchange client.
            using (ExchangeClient client = new ExchangeClient(serviceUrl, username, password))
            {
                // Retrieve mailbox information to obtain default folder URIs.
                ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;

                // Define source and destination folder URIs.
                string sourceFolderUri = mailboxInfo.InboxUri;          // Example source folder.
                string destinationFolderUri = mailboxInfo.SentItemsUri; // Example destination folder.

                // List messages in the source folder.
                ExchangeMessageInfoCollection messages = client.ListMessages(sourceFolderUri);

                foreach (ExchangeMessageInfo msgInfo in messages)
                {
                    // Move each message to the destination folder while preserving metadata.
                    client.MoveMessage(msgInfo, destinationFolderUri);
                    Console.WriteLine($"Moved message with Subject: '{msgInfo.Subject}'");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
