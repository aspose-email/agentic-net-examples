using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Tools.Search;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Exchange server connection details
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Initialize the Exchange WebDav client
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                // Build a query for messages from a specific sender
                ExchangeQueryBuilder fromBuilder = new ExchangeQueryBuilder();
                fromBuilder.From.Contains("alice@example.com");
                MailQuery fromQuery = fromBuilder.GetQuery();

                // Build a query for messages with a specific subject keyword
                ExchangeQueryBuilder subjectBuilder = new ExchangeQueryBuilder();
                subjectBuilder.Subject.Contains("Report");
                MailQuery subjectQuery = subjectBuilder.GetQuery();

                // Combine the two queries using logical OR
                MailQuery combinedQuery = fromBuilder.Or(fromQuery, subjectQuery);

                // Retrieve messages from the Inbox that match the combined query (recursive search)
                ExchangeMessageInfoCollection messages = client.ListMessages("Inbox", combinedQuery, true);

                // Output basic information about each matching message
                foreach (var msgInfo in messages)
                {
                    Console.WriteLine($"Subject: {msgInfo.Subject}, From: {msgInfo.From}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
