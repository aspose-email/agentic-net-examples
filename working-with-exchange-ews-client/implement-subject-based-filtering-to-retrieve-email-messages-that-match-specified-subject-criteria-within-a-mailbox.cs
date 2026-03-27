using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main(string[] args)
        {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

            try
            {
                // Define mailbox connection parameters
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Create credentials object
                ICredentials credentials = new NetworkCredential(username, password);

                // Initialize EWS client using the factory method
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
                {
                    // Build a query to filter messages by subject
                    ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                    builder.Subject.Contains("Invoice");
                    MailQuery query = builder.GetQuery();

                    // List messages in the Inbox that match the query
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, query, false);

                    // Iterate through the results and fetch each full message
                    foreach (ExchangeMessageInfo info in messages)
                    {
                        using (MailMessage message = client.FetchMessage(info.UniqueUri))
                        {
                            Console.WriteLine("Subject: " + message.Subject);
                            Console.WriteLine("From: " + message.From);
                            Console.WriteLine(new string('-', 40));
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