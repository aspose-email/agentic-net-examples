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
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

            try
            {
                // Define mailbox URI and credentials (replace with real values)
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                NetworkCredential credentials = new NetworkCredential("username", "password");

                // Create EWS client using the factory method
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
                {
                    try
                    {
                        // Build a simple query to fetch all messages in the Inbox
                        ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                        MailQuery query = builder.GetQuery();

                        // List messages in the Inbox folder
                        ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, query);

                        // Iterate and display basic metadata
                        foreach (ExchangeMessageInfo info in messages)
                        {
                            Console.WriteLine("Subject: " + info.Subject);
                            Console.WriteLine("From: " + info.From);
                            Console.WriteLine("Received: " + info.Date);
                            Console.WriteLine(new string('-', 40));
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Error during EWS operations: " + ex.Message);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unhandled exception: " + ex.Message);
                return;
            }
        }
    }
}