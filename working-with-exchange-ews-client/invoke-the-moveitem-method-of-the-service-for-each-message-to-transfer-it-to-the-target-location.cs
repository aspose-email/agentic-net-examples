using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using Aspose.Email;
using System;
class Program
{
    static void Main()
    {
        try
        {
            // Replace with your actual Exchange server details
            string host = "exchange.example.com";
            string username = "user@example.com";
            string password = "password";
            string domain = "example.com";


            // Skip external calls when placeholder credentials are used
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password" || domain.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Initialize the Exchange DAV client
            using (IEWSClient client = EWSClient.GetEWSClient(host, username, password, domain))
            {
                // Get mailbox information to obtain folder URIs
                ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();

                // Source folder (Inbox) and destination folder URIs
                string sourceFolderUri = mailboxInfo.InboxUri;
                string destinationFolderUri = mailboxInfo.DeletedItemsUri; // example target folder

                // List messages in the source folder
                ExchangeMessageInfoCollection messages = client.ListMessages(sourceFolderUri);

                // Move each message to the destination folder
                foreach (ExchangeMessageInfo msgInfo in messages)
                {
                    try
                    {
                        client.MoveItem(msgInfo.UniqueUri, destinationFolderUri);
                        Console.WriteLine($"Moved message with Subject: '{msgInfo.Subject}'");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to move message '{msgInfo.Subject}': {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
