using Aspose.Email.Clients.Exchange;
using Aspose.Email.Tools.Search;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Initialize the Exchange WebDAV client
            using (ExchangeClient client = new ExchangeClient("https://exchange.example.com/EWS/Exchange.asmx", "username", "password"))
            {
                // Build a query to filter messages addressed to a specific recipient
                MailQueryBuilder builder = new MailQueryBuilder();
                builder.To.Contains("recipient@example.com");
                MailQuery query = builder.GetQuery();

                // Retrieve messages from the Inbox that match the query
                ExchangeMessageInfoCollection messages = client.ListMessages("Inbox", query, false);

                // Process the filtered messages
                foreach (ExchangeMessageInfo messageInfo in messages)
                {
                    Console.WriteLine($"Subject: {messageInfo.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
