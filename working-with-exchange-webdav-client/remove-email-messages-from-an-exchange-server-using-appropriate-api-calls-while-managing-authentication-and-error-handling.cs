using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.Dav;

namespace AsposeEmailExchangeDeleteExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder credentials – replace with real values or skip execution if left unchanged.
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "username";
                string password = "password";

                // Guard against placeholder credentials to avoid unintended network calls.
                if (string.IsNullOrWhiteSpace(mailboxUri) ||
                    string.IsNullOrWhiteSpace(username) ||
                    string.IsNullOrWhiteSpace(password) ||
                    mailboxUri.Contains("example.com") ||
                    username.Equals("username", StringComparison.OrdinalIgnoreCase) ||
                    password.Equals("password", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping Exchange operations.");
                    return;
                }

                // Create and use the Exchange client.
                using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
                {
                    try
                    {
                        // Retrieve mailbox information to obtain standard folder URIs.
                        ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();

                        // List all messages in the Inbox folder.
                        ExchangeMessageInfoCollection inboxMessages = client.ListMessages(mailboxInfo.InboxUri);

                        // Move each message to the Deleted Items folder instead of deleting directly.
                        foreach (ExchangeMessageInfo messageInfo in inboxMessages)
                        {
                            try
                            {
                                client.MoveMessage(messageInfo, mailboxInfo.DeletedItemsUri);
                                Console.WriteLine($"Moved message '{messageInfo.Subject}' to Deleted Items.");
                            }
                            catch (Exception ex)
                            {
                                Console.Error.WriteLine($"Failed to move message '{messageInfo.Subject}': {ex.Message}");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Exchange operation failed: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
