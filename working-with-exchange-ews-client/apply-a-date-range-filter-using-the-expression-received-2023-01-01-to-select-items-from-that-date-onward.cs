using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Initialize EWS client with placeholder credentials
                using (IEWSClient client = EWSClient.GetEWSClient("https://exchange.example.com/EWS/Exchange.asmx", new NetworkCredential("username", "password")))
                {
                    // Build a query to filter messages with SentDate >= 2023-01-01
                    ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                    MailQuery dateQuery = builder.SentDate.Since(new DateTime(2023, 1, 1));
                    // List messages from the Inbox that match the query
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, dateQuery, false);
                    // Output subjects of the retrieved messages
                    foreach (ExchangeMessageInfo info in messages)
                    {
                        Console.WriteLine(info.Subject);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
