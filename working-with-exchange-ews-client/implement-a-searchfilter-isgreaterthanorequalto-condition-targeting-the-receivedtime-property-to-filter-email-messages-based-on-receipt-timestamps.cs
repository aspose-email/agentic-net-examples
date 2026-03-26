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
            // Define the lower bound for the ReceivedTime (using InternalDate as the closest equivalent)
            DateTime receivedSince = DateTime.UtcNow.AddDays(-7);

            // Create an EWS client (replace the URL and credentials with real values)
            using (IEWSClient client = EWSClient.GetEWSClient("https://exchange.example.com/EWS/Exchange.asmx", new NetworkCredential("user@example.com", "password")))
            {
                // Build a query that filters messages received on or after the specified date
                ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                builder.InternalDate.Greater(receivedSince);
                MailQuery query = builder.GetQuery();

                // List messages from the Inbox that satisfy the query
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, query);

                // Output basic information for each matching message
                foreach (ExchangeMessageInfo info in messages)
                {
                    Console.WriteLine($"Subject: {info.Subject}");
                    Console.WriteLine($"From: {info.From}");
                    Console.WriteLine($"Received: {info.Date}");
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