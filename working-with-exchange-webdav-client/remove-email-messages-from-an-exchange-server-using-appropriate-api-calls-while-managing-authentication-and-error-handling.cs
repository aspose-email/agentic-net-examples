using Aspose.Email.Clients.Exchange;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Top‑level exception guard
            try
            {
                // Placeholder credentials – skip actual network call in CI environments
                string serverUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "username";
                string password = "password";

                if (serverUrl.Contains("example.com") ||
                    string.Equals(username, "username", StringComparison.OrdinalIgnoreCase) ||
                    string.Equals(password, "password", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Placeholder credentials detected – skipping Exchange operations.");
                    return;
                }

                // Connect to Exchange using WebDAV client
                try
                {
                    using (ExchangeClient client = new ExchangeClient(serverUrl, username, password))
                    {
                        // Retrieve mailbox information
                        ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;
                        string inboxUri = mailboxInfo.InboxUri;
                        string deletedItemsUri = mailboxInfo.DeletedItemsUri;

                        // List all messages in the Inbox
                        ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri);

                        // Move each message to Deleted Items folder
                        foreach (ExchangeMessageInfo msgInfo in messages)
                        {
                            try
                            {
                                client.MoveMessage(msgInfo, deletedItemsUri);
                                Console.WriteLine($"Moved message '{msgInfo.Subject}' to Deleted Items.");
                            }
                            catch (Exception moveEx)
                            {
                                Console.Error.WriteLine($"Failed to move message '{msgInfo.Subject}': {moveEx.Message}");
                            }
                        }
                    }
                }
                catch (Exception connEx)
                {
                    Console.Error.WriteLine($"Error connecting to Exchange server: {connEx.Message}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
