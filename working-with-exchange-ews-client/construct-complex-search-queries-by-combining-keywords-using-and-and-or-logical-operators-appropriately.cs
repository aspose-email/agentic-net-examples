using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(
                "https://exchange.example.com/EWS/Exchange.asmx",
                new NetworkCredential("username", "password")))
            {
                // Build a query for messages with Subject containing "Report"
                ExchangeQueryBuilder subjectBuilder = new ExchangeQueryBuilder();
                subjectBuilder.Subject.Contains("Report");
                MailQuery subjectQuery = subjectBuilder.GetQuery();

                // Build a query for messages from Alice
                ExchangeQueryBuilder aliceBuilder = new ExchangeQueryBuilder();
                aliceBuilder.From.Contains("alice@example.com");
                MailQuery aliceQuery = aliceBuilder.GetQuery();

                // Build a query for messages from Bob
                ExchangeQueryBuilder bobBuilder = new ExchangeQueryBuilder();
                bobBuilder.From.Contains("bob@example.com");
                MailQuery bobQuery = bobBuilder.GetQuery();

                // Combine Alice and Bob queries with OR
                ExchangeQueryBuilder orBuilder = new ExchangeQueryBuilder();
                MailQuery fromOrQuery = orBuilder.Or(aliceQuery, bobQuery);

                // Combine Subject query (AND) with the ORed From query
                // Since the default combination is AND, we add both filters to a new builder
                ExchangeQueryBuilder finalBuilder = new ExchangeQueryBuilder();
                finalBuilder.Subject.Contains("Report");
                // Use Or method to incorporate the ORed From condition
                MailQuery combinedQuery = finalBuilder.Or(finalBuilder.GetQuery(), fromOrQuery);

                // List messages using the combined query
                ExchangeMessageInfoCollection messages = client.ListMessages(
                    client.MailboxInfo.InboxUri,
                    combinedQuery,
                    false);

                foreach (ExchangeMessageInfo info in messages)
                {
                    Console.WriteLine($"Subject: {info.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
