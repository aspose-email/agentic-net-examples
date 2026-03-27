using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Advanced Query Syntax (AQS) allows building complex search expressions.
            // Example: Find messages where the subject contains "Report" OR the sender is "alice@example.com".

            // Create an AQS query builder.
            ExchangeAdvancedSyntaxQueryBuilder aqsBuilder = new ExchangeAdvancedSyntaxQueryBuilder();

            // Build individual query parts.
            MailQuery subjectPart = aqsBuilder.Subject.Contains("Report");
            MailQuery fromPart = aqsBuilder.From.Contains("alice@example.com");

            // Combine the parts with OR to create a composite query.
            MailQuery combinedQuery = aqsBuilder.Or(subjectPart, fromPart);

            // The combined query can be used directly with the client.
            MailQuery query = combinedQuery;

            // Initialize the EWS client (replace placeholders with real values).
            using (IEWSClient client = EWSClient.GetEWSClient(
                "https://exchange.example.com/EWS/Exchange.asmx",
                new NetworkCredential("user@example.com", "password")))
            {
                // List messages in the Inbox that match the AQS query.
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, query);

                // Output basic metadata for each matching message.
                foreach (ExchangeMessageInfo info in messages)
                {
                    Console.WriteLine("Subject: " + info.Subject);
                    Console.WriteLine("From: " + info.From);
                    Console.WriteLine("Received: " + info.Date);
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}