using Aspose.Email.Clients.Exchange;
using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        // Author: Aspose.Email example - filter messages by today's sent date using Exchange WebDav client.

        // Prepare connection parameters (replace with real values when running).
        string serviceUrl = "http://exchange.example.com/EWS/Exchange.asmx";
        string userName = "user@example.com";
        string password = "password";

        // Skip external calls when placeholder credentials are used
        if (serviceUrl.Contains("example.com") || userName.Contains("example.com") || password == "password")
        {
            Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
            return;
        }

        try
        {
            // Initialize the Exchange WebDav client.
            using (ExchangeClient client = new ExchangeClient(serviceUrl, new NetworkCredential(userName, password)))
            {
                // Build a query that matches messages whose SentDate equals the current system date.
                string todayString = DateTime.Now.ToString("dd-MMM-yyyy", System.Globalization.CultureInfo.InvariantCulture);
                string queryString = $"'SentDate' = '{todayString}'";

                // Retrieve messages that satisfy the query.
                // ListMessages returns a collection of MessageInfo objects.
                ExchangeMessageInfoCollection messages = client.ListMessages(queryString);

                int count = 0;
                foreach (var msgInfo in messages)
                {
                    count++;
                    Console.WriteLine($"Subject: {msgInfo.Subject}");
                }

                Console.WriteLine($"Total messages found for date {todayString}: {count}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
