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
            // Initialize EWS client with placeholder credentials
            using (IEWSClient client = EWSClient.GetEWSClient(
                "https://example.com/EWS/Exchange.asmx",
                new NetworkCredential("user@example.com", "password")))
            {
                // Build an AQS query: Subject contains "Invoice" AND From contains "john@example.com"
                ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                builder.Subject.Contains("Invoice");
                builder.From.Contains("john@example.com");
                MailQuery query = builder.GetQuery();

                // List messages from the Inbox that match the query (non-recursive)
                ExchangeMessageInfoCollection messages = client.ListMessages(
                    client.MailboxInfo.InboxUri,
                    query,
                    false);

                // Output basic metadata for each matching message
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