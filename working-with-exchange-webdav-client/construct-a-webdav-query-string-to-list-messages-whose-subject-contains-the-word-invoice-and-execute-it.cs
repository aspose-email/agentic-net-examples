using Aspose.Email;
using System;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Tools.Search;

namespace AsposeEmailWebDavQuery
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder connection settings
                string exchangeUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Skip execution when placeholder credentials are detected
                if (exchangeUri.Contains("example.com") || username.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                    return;
                }

                // Create and connect the Exchange WebDAV client
                try
                {
                    using (ExchangeClient client = new ExchangeClient(exchangeUri, username, password))
                    {
                        // Build a query to find messages with "Invoice" in the subject
                        ExchangeQueryBuilder queryBuilder = new ExchangeQueryBuilder();
                        queryBuilder.Subject.Contains("Invoice");
                        MailQuery mailQuery = queryBuilder.GetQuery();

                        // List messages from the Inbox that match the query
                        ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, mailQuery.ToString());

                        // Output the subject of each matching message
                        foreach (Aspose.Email.Clients.Exchange.ExchangeMessageInfo messageInfo in messages)
                        {
                            Console.WriteLine("Subject: " + messageInfo.Subject);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error connecting to Exchange server: " + ex.Message);
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unexpected error: " + ex.Message);
            }
        }
    }
}
