using Aspose.Email.Tools.Search;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Create EWS client with connection safety
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
            {
                // Build AQS query for unread high‑importance emails
                ExchangeAdvancedSyntaxQueryBuilder queryBuilder = new ExchangeAdvancedSyntaxQueryBuilder();

                // Set conditions (IsRead = false AND Importance = High)
                queryBuilder.IsRead.Equals(false);
                queryBuilder.Importance.Equals("High");

                MailQuery query = queryBuilder.GetQuery();

                // Retrieve messages from the Inbox that match the query
                try
                {
                    string inboxUri = client.MailboxInfo.InboxUri;
                    ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri, query);

                    foreach (var msgInfo in messages)
                    {
                        Console.WriteLine($"Subject: {msgInfo.Subject}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error listing messages: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
