using Aspose.Email.Tools.Search;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – skip actual network call in CI environments
            string exchangeUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            if (exchangeUrl.Contains("example.com") || username == "username")
            {
                Console.Error.WriteLine("Placeholder credentials detected – skipping Exchange connection.");
                return;
            }

            // Create and connect the Exchange WebDAV client
            using (ExchangeClient client = new ExchangeClient(exchangeUrl, username, password))
            {
                try
                {
                    // Build a query that selects only unread messages (IsRead flag not set)
                    ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                    builder.HasNoFlags(ExchangeMessageFlag.IsRead);
                    MailQuery query = builder.GetQuery();

                    // List unread messages from the Inbox folder
                    ExchangeMessageInfoCollection unreadMessages = client.ListMessages(client.MailboxInfo.InboxUri, query.ToString());

                    Console.WriteLine($"Found {unreadMessages.Count} unread message(s) in the Inbox.");

                    // Example: output the subject of each unread message
                    foreach (var msgInfo in unreadMessages)
                    {
                        Console.WriteLine($"Subject: {msgInfo.Subject}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during Exchange operation: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
