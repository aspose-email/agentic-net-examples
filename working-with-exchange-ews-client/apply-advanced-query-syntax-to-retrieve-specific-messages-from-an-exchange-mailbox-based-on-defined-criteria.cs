using Aspose.Email.Tools.Search;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize credentials for the Exchange server
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create the EWS client using the factory method
            using (IEWSClient client = EWSClient.GetEWSClient("https://exchange.example.com/EWS/Exchange.asmx", credentials))
            {
                // Build an advanced query: subject contains "Report" AND from contains "john@example.com"
                ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                builder.Subject.Contains("Report");
                builder.From.Contains("john@example.com");
                MailQuery query = builder.GetQuery();

                // Retrieve messages from the Inbox that match the query
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, query, false);

                // Output the subject of each matching message
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
