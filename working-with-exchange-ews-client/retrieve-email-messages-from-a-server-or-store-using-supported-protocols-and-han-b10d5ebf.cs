using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

namespace EmailRetrievalExample
{
    class Program
    {
        static void Main(string[] args)
        {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

            try
            {
                // Define Exchange service URL and credentials (replace with real values)
                string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Create the EWS client using the factory method
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, new NetworkCredential(username, password)))
                {
                    // Build a query to retrieve all messages (no filters)
                    ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                    MailQuery query = builder.GetQuery();

                    // List messages from the Inbox folder
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, query);

                    // Iterate through each message and display its subject
                    foreach (ExchangeMessageInfo info in messages)
                    {
                        using (MailMessage message = client.FetchMessage(info.UniqueUri))
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
}