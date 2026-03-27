using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Exchange server connection details (replace with real values)
            string exchangeUrl = "https://exchange.example.com/exchange/username@domain.com/";
            string userName = "username";
            string password = "password";

            // Create and use the ExchangeClient inside a using block to ensure disposal
            using (Aspose.Email.Clients.Exchange.Dav.ExchangeClient client = new Aspose.Email.Clients.Exchange.Dav.ExchangeClient(exchangeUrl, new NetworkCredential(userName, password)))
            {
                try
                {
                    // Build a query to filter messages (e.g., subject contains "Report")
                    Aspose.Email.Clients.Exchange.ExchangeQueryBuilder builder = new Aspose.Email.Clients.Exchange.ExchangeQueryBuilder();
                    builder.Subject.Contains("Report");
                    MailQuery query = builder.GetQuery();

                    // List messages from the Inbox folder that match the query (non‑recursive)
                    Aspose.Email.Clients.Exchange.ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, query, false);

                    // Output basic information for each message
                    foreach (Aspose.Email.Clients.Exchange.ExchangeMessageInfo info in messages)
                    {
                        Console.WriteLine("Subject: " + info.Subject);
                        Console.WriteLine("From: " + (info.From != null ? info.From.ToString() : "Unknown"));
                        Console.WriteLine("Received: " + info.Date);
                        Console.WriteLine(new string('-', 40));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error during message retrieval: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Failed to connect to Exchange server: " + ex.Message);
        }
    }
}