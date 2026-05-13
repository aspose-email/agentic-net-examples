using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – skip actual network call in CI environments
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            if (mailboxUri.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping Exchange call.");
                return;
            }

            // Create the Exchange WebDav client
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // Build a query to fetch messages received in the last 30 days
                MailQueryBuilder builder = new MailQueryBuilder();
                DateTime fromDate = DateTime.Now.AddDays(-30);
                MailQuery dateQuery = builder.InternalDate.Since(fromDate);

                // Retrieve messages from the Inbox that match the date filter
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, dateQuery.ToString()); // recursive = true

                // Output basic information about each message
                foreach (var info in messages)
                {
                    Console.WriteLine($"Subject: {info.Subject}");
                    Console.WriteLine($"Received (InternalDate): {info.InternalDate}");
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
