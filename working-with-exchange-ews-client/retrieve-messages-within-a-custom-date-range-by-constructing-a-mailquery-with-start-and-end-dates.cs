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
            // Server URI and credentials
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";

            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Define the custom date range
                DateTime startDate = new DateTime(2023, 1, 1);
                DateTime endDate = new DateTime(2023, 12, 31);

                // Build a MailQuery for the internal date range
                MailQueryBuilder builder = new MailQueryBuilder();
                builder.InternalDate.Since(startDate);
                builder.InternalDate.Before(endDate);
                MailQuery query = builder.GetQuery();

                // Retrieve messages from the Inbox that match the query
                ExchangeMessageInfoCollection messages = client.ListMessages("Inbox", query);

                // Display the subject of each retrieved message
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
