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
                // Define connection parameters (replace with real values or keep placeholders)
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Create the EWS client using the factory method (returns IEWSClient)
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
                {
                    // Build a simple query to find messages with a specific subject keyword
                    ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                    builder.Subject.Contains("Invoice");
                    MailQuery query = builder.GetQuery();

                    // List messages from the Inbox that match the query
                    var messages = client.ListMessages(client.MailboxInfo.InboxUri, query);

                    // Output basic metadata for each message
                    foreach (var info in messages)
                    {
                        Console.WriteLine($"Subject: {info.Subject}");
                        Console.WriteLine($"From: {info.From}");
                        Console.WriteLine($"Received: {info.Date}");
                        Console.WriteLine(new string('-', 40));
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