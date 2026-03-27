using System.Net;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Tools.Search;

namespace EmailDateFilterExample
{
    class Program
    {
        static void Main(string[] args)
        {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

            try
            {
                // Initialize EWS client (replace with actual server URL and credentials)
                using (IEWSClient client = EWSClient.GetEWSClient("https://exchange.example.com/EWS/Exchange.asmx", new System.Net.NetworkCredential("username", "password")))
                {
                    // Define the date range
                    DateTime startDate = new DateTime(2023, 1, 1);
                    DateTime endDate = new DateTime(2023, 2, 1);

                    // Build the query: messages sent on or after startDate and before endDate (exclusive upper bound)
                    ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                    builder.SentDate.Greater(startDate);
                    builder.SentDate.Before(endDate);
                    MailQuery query = builder.GetQuery();

                    // Retrieve messages from the Inbox that match the query
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, query, false);

                    // Output basic information for each message
                    foreach (ExchangeMessageInfo info in messages)
                    {
                        Console.WriteLine("Subject: " + info.Subject);
                        Console.WriteLine("From: " + info.From);
                        Console.WriteLine("Sent: " + info.Date);
                        Console.WriteLine(new string('-', 40));
                    }

                    // Example of inclusive upper bound: add one day to make the end date inclusive
                    DateTime inclusiveEndDate = endDate.AddDays(1);
                    ExchangeQueryBuilder inclusiveBuilder = new ExchangeQueryBuilder();
                    inclusiveBuilder.SentDate.Greater(startDate);
                    inclusiveBuilder.SentDate.Before(inclusiveEndDate);
                    MailQuery inclusiveQuery = inclusiveBuilder.GetQuery();

                    ExchangeMessageInfoCollection inclusiveMessages = client.ListMessages(client.MailboxInfo.InboxUri, inclusiveQuery, false);
                    Console.WriteLine("Inclusive upper bound results count: " + inclusiveMessages.Count);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}