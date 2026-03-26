using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;
using Aspose.Email.Clients.Exchange;
using Aspose.Email;
using System;
using System.Net;

class Program
{
    static void Main()
    {
        try
        {
            // Exchange WebDAV service URL and credentials (replace with real values)
            string serviceUrl = "https://exchange.example.com/exchange/username@domain.com/";
            NetworkCredential credential = new NetworkCredential("username", "password");

            // Connect to the Exchange server using WebDAV client
            using (Aspose.Email.Clients.Exchange.Dav.ExchangeClient client = new Aspose.Email.Clients.Exchange.Dav.ExchangeClient(serviceUrl, credential))
            {
                // Build a query to filter messages whose subject contains "Invoice"
                Aspose.Email.Clients.Exchange.ExchangeQueryBuilder builder = new Aspose.Email.Clients.Exchange.ExchangeQueryBuilder();
                builder.Subject.Contains("Invoice");
                Aspose.Email.Tools.Search.MailQuery query = builder.GetQuery();

                // Retrieve messages from the Inbox that match the query
                Aspose.Email.Clients.Exchange.ExchangeMessageInfoCollection messages = client.ListMessages("Inbox", query, true);

                // Iterate over the matching messages and display their subjects
                foreach (Aspose.Email.Clients.Exchange.ExchangeMessageInfo info in messages)
                {
                    // Fetch the full message to access its properties
                    using (Aspose.Email.MailMessage message = client.FetchMessage(info.UniqueUri))
                    {
                        Console.WriteLine("Subject: " + message.Subject);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}