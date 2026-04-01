using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder connection details
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip real network call when placeholders are used
            if (serviceUrl.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping actual server connection.");
                return;
            }

            NetworkCredential credentials = new NetworkCredential(username, password);

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
            {
                // Define date range (last 7 days)
                DateTime startDate = DateTime.Today.AddDays(-7);
                DateTime endDate = DateTime.Today;

                // Build query using internal date
                ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                builder.InternalDate.Since(startDate);
                builder.InternalDate.Before(endDate);
                MailQuery query = builder.GetQuery();

                // Retrieve messages from Inbox matching the date criteria
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, query);

                foreach (ExchangeMessageInfo info in messages)
                {
                    Console.WriteLine($"Subject: {info.Subject}, InternalDate: {info.InternalDate}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
