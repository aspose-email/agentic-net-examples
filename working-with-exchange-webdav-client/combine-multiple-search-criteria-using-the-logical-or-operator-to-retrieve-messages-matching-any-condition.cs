using Aspose.Email.Tools.Search;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Initialize the Exchange WebDAV client
            using (ExchangeClient client = new ExchangeClient(
                "https://exchange.example.com/EWS/Exchange.asmx",
                "username",
                "password"))
            {
                // Build first query: messages from a specific sender
                ExchangeQueryBuilder builderFrom = new ExchangeQueryBuilder();
                builderFrom.From.Contains("alice@example.com");
                MailQuery queryFrom = builderFrom.GetQuery();

                // Build second query: messages with a specific subject keyword
                ExchangeQueryBuilder builderSubject = new ExchangeQueryBuilder();
                builderSubject.Subject.Contains("Report");
                MailQuery querySubject = builderSubject.GetQuery();

                // Combine the two queries using logical OR
                ExchangeQueryBuilder builderCombined = new ExchangeQueryBuilder();
                MailQuery combinedQuery = builderCombined.Or(queryFrom, querySubject);

                // Retrieve messages from the Inbox that match either condition
                ExchangeMessageInfoCollection messages = client.ListMessages(
                    client.MailboxInfo.InboxUri,
                    combinedQuery,
                    false); // non‑recursive

                // Display the subject of each matching message
                foreach (var info in messages)
                {
                    using (MailMessage message = client.FetchMessage(info.UniqueUri))
                    {
                        Console.WriteLine(message.Subject);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
