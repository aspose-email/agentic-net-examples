using Aspose.Email;
using System;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Exchange service URL and credentials
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create the EWS client (IEWSClient) and ensure it is disposed properly
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Retrieve mailbox folder URIs
                ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();
                string inboxUri = mailboxInfo.InboxUri;
                string archiveUri = mailboxInfo.InboxUri; // Replace with actual target folder URI

                // List messages in the source folder (Inbox)
                ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri);
                if (messages.Count > 0)
                {
                    // Take the first message and move it to the target folder
                    string itemUri = messages[0].UniqueUri;
                    string movedItemUri = client.MoveItem(itemUri, archiveUri);
                    Console.WriteLine($"Message moved successfully. New URI: {movedItemUri}");
                }
                else
                {
                    Console.WriteLine("No messages found in the source folder.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
