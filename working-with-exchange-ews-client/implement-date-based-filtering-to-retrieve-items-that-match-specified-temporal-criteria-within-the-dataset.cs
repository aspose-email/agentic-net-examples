using Aspose.Email.Tools.Search;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection details – replace with real values when testing.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create the EWS client using the factory method.
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
            {
                // Build a query to retrieve messages sent in the last 7 days.
                ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                builder.SentDate.Since(DateTime.Today.AddDays(-7));
                MailQuery query = builder.GetQuery();

                // List messages from the Inbox that match the query (non‑recursive).
                ExchangeMessageInfoCollection infos = client.ListMessages(client.MailboxInfo.InboxUri, query, false);

                foreach (ExchangeMessageInfo info in infos)
                {
                    // Fetch the full message using its unique URI.
                    using (MailMessage message = client.FetchMessage(info.UniqueUri))
                    {
                        Console.WriteLine($"Subject: {message.Subject}");
                        Console.WriteLine($"Sent: {message.Date}");
                        Console.WriteLine(new string('-', 40));
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
