using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        // Placeholder values – replace with real server URL and credentials.
        string serviceUrl = "https://your-ews-server/EWS/Exchange.asmx";
        string username = "user@example.com";
        string password = "password";

        // Guard: skip network calls when placeholders are still present.
        if (serviceUrl.Contains("your-ews-server") ||
            username.Contains("example.com") ||
            password == "password")
        {
            Console.WriteLine("Placeholder credentials detected. Skipping EWS operations.");
            return;
        }

        try
        {
            // Create and connect the EWS client.
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Retrieve mailbox information to get the Inbox folder URI.
                ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();
                string inboxUri = mailboxInfo.InboxUri;

                // List all messages in the Inbox.
                ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri);

                // Delete each message permanently.
                int deletedCount = 0;
                foreach (ExchangeMessageInfo msgInfo in messages)
                {
                    client.DeleteItem(msgInfo.UniqueUri, DeletionOptions.DeletePermanently);
                    deletedCount++;
                }

                Console.WriteLine($"{deletedCount} message(s) deleted permanently.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
