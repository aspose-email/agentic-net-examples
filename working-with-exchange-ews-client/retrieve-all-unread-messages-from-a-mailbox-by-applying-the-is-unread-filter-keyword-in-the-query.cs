using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Tools.Search;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Define the EWS service URL and credentials.
                string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                NetworkCredential credentials = new NetworkCredential("username", "password");

                // Create the EWS client. The factory returns an IEWSClient instance.
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
                {
                    // Build a query that selects messages which are NOT marked as read (i.e., unread).
                    ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                    builder.HasNoFlags(ExchangeMessageFlag.IsRead);
                    MailQuery query = builder.GetQuery();

                    // Retrieve unread messages from the Inbox folder.
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, query, false);

                    // Output basic information for each unread message.
                    foreach (ExchangeMessageInfo info in messages)
                    {
                        Console.WriteLine($"Subject: {info.Subject}");
                        Console.WriteLine($"From: {info.From}");
                        Console.WriteLine($"Date: {info.Date}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
            catch (Exception ex)
            {
                // Write any errors to the error stream.
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
