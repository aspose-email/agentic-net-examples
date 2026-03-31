using System;
using System.Net;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details – replace with real values when running in production.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Simple guard to avoid making real network calls with placeholder credentials.
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping mailbox cleanup.");
                return;
            }

            // Create the Exchange client (Dav) and ensure it is disposed properly.
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // Retrieve mailbox information (folders URIs).
                ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;

                // List messages in the Inbox folder.
                ExchangeMessageInfoCollection inboxMessages = client.ListMessages(mailboxInfo.InboxUri);

                foreach (ExchangeMessageInfo messageInfo in inboxMessages)
                {
                    // Define your own criteria for unwanted messages.
                    // Example: messages whose subject contains the word "Unwanted".
                    if (messageInfo.Subject != null && messageInfo.Subject.IndexOf("Unwanted", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        try
                        {
                            // Move the unwanted message to the Deleted Items folder.
                            client.MoveMessage(messageInfo, mailboxInfo.DeletedItemsUri);
                            Console.WriteLine($"Moved message '{messageInfo.Subject}' to Deleted Items.");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to move message '{messageInfo.Subject}': {ex.Message}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
