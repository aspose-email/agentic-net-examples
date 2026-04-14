using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize Exchange client (replace with actual server details)
            string hostUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (hostUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            using (IEWSClient client = EWSClient.GetEWSClient(hostUri, new NetworkCredential(username, password)))
            {
                // Build a query to find messages whose subject contains the keyword "Invoice"
                ExchangeQueryBuilder queryBuilder = new ExchangeQueryBuilder();
                queryBuilder.Subject.Contains("Invoice");
                MailQuery query = queryBuilder.GetQuery();

                // List messages in the Inbox folder that match the query (non‑recursive)
                ExchangeMessageInfoCollection messages = client.ListMessages("Inbox", query);

                foreach (ExchangeMessageInfo info in messages)
                {
                    // Fetch the full message to read its subject
                    MailMessage message = client.FetchMessage(info.UniqueUri);
                    Console.WriteLine($"Subject: {message.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
