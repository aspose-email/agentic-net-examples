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
            // Placeholder connection details
            string serviceUrl = "https://exchange.example.com/EWS";
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholders are detected
            if (serviceUrl.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping archive operation.");
                return;
            }

            // Create the EWS client using the factory method
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, new NetworkCredential(username, password)))
            {
                // Build a query to select messages that meet the criteria (e.g., subject contains "Report")
                MailQueryBuilder builder = new MailQueryBuilder();
                builder.Subject.Contains("Report");
                MailQuery query = builder.GetQuery();

                // Retrieve messages from the Inbox that match the query
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, query);

                // Archive each matching message
                foreach (ExchangeMessageInfo info in messages)
                {
                    client.ArchiveItem(client.MailboxInfo.InboxUri, info.UniqueUri);
                    Console.WriteLine($"Archived message: {info.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
