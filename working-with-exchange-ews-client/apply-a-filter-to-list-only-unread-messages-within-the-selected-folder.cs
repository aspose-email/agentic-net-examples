using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize Exchange client (EWS)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Build a query to filter unread messages
                ExchangeQueryBuilder queryBuilder = new ExchangeQueryBuilder();
                MailQuery unreadQuery = queryBuilder.HasNoFlags(ExchangeMessageFlag.IsRead);

                // Specify the folder (e.g., Inbox)
                string folderUri = client.MailboxInfo.InboxUri;

                // List unread messages in the folder
                ExchangeMessageInfoCollection messages = client.ListMessages(folderUri, unreadQuery);

                Console.WriteLine($"Unread messages count: {messages.Count}");
                foreach (ExchangeMessageInfo msgInfo in messages)
                {
                    Console.WriteLine($"Subject: {msgInfo.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
