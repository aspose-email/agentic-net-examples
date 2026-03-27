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
            // Mailbox URI must be a string, not a Uri object
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create the EWS client using the factory method
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Build first part of the query: Subject contains "Report"
                ExchangeQueryBuilder subjectBuilder = new ExchangeQueryBuilder();
                subjectBuilder.Subject.Contains("Report");
                MailQuery subjectQuery = subjectBuilder.GetQuery();

                // Build second part of the query: From contains "alice@example.com"
                ExchangeQueryBuilder fromBuilder = new ExchangeQueryBuilder();
                fromBuilder.From.Contains("alice@example.com");
                MailQuery fromQuery = fromBuilder.GetQuery();

                // Combine the two queries with OR
                MailQuery combinedQuery = subjectBuilder.Or(subjectQuery, fromQuery);

                // List messages in the Inbox that match the combined query
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, combinedQuery, false);

                foreach (ExchangeMessageInfo info in messages)
                {
                    Console.WriteLine($"Subject: {info.Subject}");
                    Console.WriteLine($"From: {info.From}");
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
