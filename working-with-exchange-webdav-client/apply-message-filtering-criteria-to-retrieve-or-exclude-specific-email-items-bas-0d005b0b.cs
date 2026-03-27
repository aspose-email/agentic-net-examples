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
                // Define the Exchange Web Services (EWS) endpoint and credentials.
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                NetworkCredential credentials = new NetworkCredential("username", "password");

                // Create the EWS client using the factory method.
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
                {
                    try
                    {
                        // Build a query to filter messages where the subject contains "Report"
                        // and the sender's address contains "alice@example.com".
                        ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                        builder.Subject.Contains("Report");
                        builder.From.Contains("alice@example.com");
                        MailQuery query = builder.GetQuery();

                        // Retrieve messages from the Inbox that match the query.
                        ExchangeMessageInfoCollection messages = client.ListMessages("Inbox", query, false);

                        // Output basic metadata for each matching message.
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
                        Console.Error.WriteLine("Operation error: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unhandled exception: " + ex.Message);
            }
        }
    }
}