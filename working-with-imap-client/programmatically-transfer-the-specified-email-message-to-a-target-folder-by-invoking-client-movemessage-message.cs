using Aspose.Email;
using System;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Author note: Example demonstrates moving an email on an Exchange server using ExchangeClient.
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create and use the Exchange client.
            using (ExchangeClient client = new ExchangeClient(serviceUrl, username, password))
            {
                // Retrieve mailbox information to obtain folder URIs.
                ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();

                // Source folder (Inbox) and destination folder (Deleted Items) URIs.
                string sourceFolderUri = mailboxInfo.InboxUri;
                string destinationFolderUri = mailboxInfo.DeletedItemsUri;

                // List messages in the source folder.
                ExchangeMessageInfoCollection messages = client.ListMessages(sourceFolderUri);
                if (messages == null || messages.Count == 0)
                {
                    Console.WriteLine("No messages found in the source folder.");
                    return;
                }

                // Take the first message to move.
                ExchangeMessageInfo messageInfo = messages[0];

                // Move the message to the destination folder.
                client.MoveMessage(messageInfo, destinationFolderUri);
                Console.WriteLine($"Message '{messageInfo.Subject}' moved to the destination folder.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
