using Aspose.Email.Clients.Exchange;
using Aspose.Email.Tools.Search;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            try
            {
                string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                NetworkCredential credentials = new NetworkCredential("username", "password");
                using (ExchangeClient client = new ExchangeClient(serviceUrl, credentials))
                {
                    // Build a query to select messages where the internal date equals today
                    MailQueryBuilder builder = new MailQueryBuilder();
                    builder.InternalDate.Equals(DateTime.Today);
                    MailQuery query = builder.GetQuery();

                    // List messages from the Inbox that match the query
                    ExchangeMessageInfoCollection messages = client.ListMessages("Inbox", query, false);
                    foreach (ExchangeMessageInfo messageInfo in messages)
                    {
                        Console.WriteLine($"Subject: {messageInfo.Subject}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
