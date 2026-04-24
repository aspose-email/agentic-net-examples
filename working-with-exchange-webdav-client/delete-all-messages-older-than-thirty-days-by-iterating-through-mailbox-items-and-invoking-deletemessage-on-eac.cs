using Aspose.Email.Clients.Exchange;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details – replace with real values.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against executing with placeholder credentials/host.
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create and use the Exchange client.
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                try
                {
                    // Retrieve all messages from the Inbox.
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);

                    // Define the cutoff date (30 days ago).
                    DateTime cutoffDate = DateTime.Now.AddDays(-30);

                    // Destination folder for old messages (Deleted Items).
                    string deletedItemsUri = client.MailboxInfo.DeletedItemsUri;

                    foreach (ExchangeMessageInfo msgInfo in messages)
                    {
                        // Some messages may not have a Date; skip those.
                        if (msgInfo.InternalDate < cutoffDate)
                        {
                            try
                            {
                                // Move the old message to Deleted Items instead of deleting it.
                                client.MoveMessage(msgInfo, deletedItemsUri);
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
                    Console.Error.WriteLine($"Error during mailbox processing: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
