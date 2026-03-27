using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Tools.Search;

namespace AsposeEmailDateFilterExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define EWS service URL and credentials (replace with real values)
                string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                NetworkCredential credentials = new NetworkCredential("username", "password");

                // Create and connect the EWS client safely
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
                {
                    // Build a query to retrieve messages sent in the last 7 days
                    ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                    builder.SentDate.Greater(DateTime.UtcNow.AddDays(-7));
                    MailQuery query = builder.GetQuery();

                    // List messages from the Inbox that match the date filter
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, query);

                    // Output basic information for each matching message
                    foreach (ExchangeMessageInfo messageInfo in messages)
                    {
                        Console.WriteLine("Subject: " + messageInfo.Subject);
                        Console.WriteLine("From: " + messageInfo.From);
                        Console.WriteLine("Sent: " + messageInfo.Date);
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
