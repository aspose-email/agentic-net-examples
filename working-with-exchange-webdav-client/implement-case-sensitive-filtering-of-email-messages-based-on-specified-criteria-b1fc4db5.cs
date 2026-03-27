using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Tools.Search;

namespace EmailCaseSensitiveFilter
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

                // Create the EWS client inside a using block to ensure disposal
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, new NetworkCredential(username, password)))
                {
                    // Build a case‑sensitive query: subject must contain the exact text "Invoice"
                    ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                    builder.Subject.Contains("Invoice");
                    MailQuery query = builder.GetQuery();

                    // List messages from the Inbox that match the query
                    // The third parameter indicates whether to include subfolders (false = only Inbox)
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, query, false);

                    // Output the subject of each matching message
                    foreach (ExchangeMessageInfo info in messages)
                    {
                        Console.WriteLine("Subject: " + info.Subject);
                    }
                }
            }
            catch (Exception ex)
            {
                // Write any errors to the error stream and exit gracefully
                Console.Error.WriteLine("Error: " + ex.Message);
                return;
            }
        }
    }
}