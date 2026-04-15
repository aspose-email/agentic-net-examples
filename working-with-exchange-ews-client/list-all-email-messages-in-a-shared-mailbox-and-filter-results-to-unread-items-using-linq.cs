using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using System;
using System.Linq;
using Aspose.Email;
class Program
{
    static void Main()
    {
        try
        {
            // Initialize Exchange client for the shared mailbox
            string exchangeUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string userName = "user@example.com";
            string password = "password";

            using (IEWSClient client = EWSClient.GetEWSClient(exchangeUri, userName, password))
            {
                // Build the folder URI for the shared mailbox's Inbox
                string sharedMailbox = "shared@example.com";

                // Skip external calls when placeholder credentials are used
                if (exchangeUri.Contains("example.com") || userName.Contains("example.com") || password == "password" || sharedMailbox.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                string inboxFolderUri = $"{sharedMailbox}/Inbox";

                // Retrieve all messages from the Inbox (including subfolders)
                ExchangeMessageInfoCollection allMessages = client.ListMessages(inboxFolderUri, true);

                // Filter unread messages using LINQ
                var unreadMessages = allMessages.Where(msg => !msg.IsRead);

                // Output subject of each unread message
                foreach (var message in unreadMessages)
                {
                    Console.WriteLine($"Subject: {message.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
