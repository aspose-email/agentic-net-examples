using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main(string[] args)
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Define connection parameters (replace with real values as needed)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create credentials
            NetworkCredential credentials = new NetworkCredential(username, password);

            // Initialize EWS client inside a using block to ensure disposal
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Define the date range for filtering (last 7 days)
                DateTime startDate = DateTime.UtcNow.AddDays(-7);
                DateTime endDate = DateTime.UtcNow;

                // Build the query using ExchangeQueryBuilder
                ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                builder.InternalDate.Greater(startDate);
                builder.InternalDate.Before(endDate);
                MailQuery query = builder.GetQuery();

                // Retrieve messages from the Inbox that match the date criteria
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, query);

                // Output basic metadata for each matching message
                foreach (ExchangeMessageInfo info in messages)
                {
                    Console.WriteLine("Subject: " + info.Subject);
                    Console.WriteLine("From: " + info.From);
                    Console.WriteLine("Date: " + info.Date);
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        catch (Exception ex)
        {
            // Write any errors to the error console without crashing the application
            Console.Error.WriteLine(ex.Message);
        }
    }
}