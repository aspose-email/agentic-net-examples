using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder values – replace with real credentials when available.
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against executing with placeholder credentials.
            if (serviceUrl.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder service URL detected. Skipping network operation.");
                return;
            }

            // Create the EWS client.
            IEWSClient client = null;
            try
            {
                client = EWSClient.GetEWSClient(serviceUrl, new NetworkCredential(username, password));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            // Destination folder URI – placeholder.
            string destinationFolderUri = client.MailboxInfo.InboxUri; // Example: move to Inbox.

            // List messages in the source folder (e.g., Drafts).
            ExchangeMessageInfoCollection messages = null;
            try
            {
                messages = client.ListMessages(client.MailboxInfo.DraftsUri);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to list messages: {ex.Message}");
                return;
            }

            // Move each message to the destination folder.
            foreach (ExchangeMessageInfo info in messages)
            {
                try
                {
                    // Use the message's UniqueUri with the MoveItem method.
                    string movedUri = client.MoveItem(info.UniqueUri, destinationFolderUri);
                    Console.WriteLine($"Moved message {info.UniqueUri} to {destinationFolderUri}. New URI: {movedUri}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to move message {info.UniqueUri}: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
