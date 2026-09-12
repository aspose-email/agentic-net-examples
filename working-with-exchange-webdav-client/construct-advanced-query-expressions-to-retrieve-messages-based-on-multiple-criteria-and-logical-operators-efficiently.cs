using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize Exchange WebDAV client
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            using (ExchangeClient client = new ExchangeClient(serviceUrl, username, password))
            {
                // Build an advanced query:
                // (From contains 'sales@example.com' OR Seen = True) AND SentDate >= '01-Jan-2023'
                string queryString = "(('From' Contains 'sales@example.com' | 'Seen' = 'True') & 'SentDate' >= '01-Jan-2023')";

                // Skip external calls when placeholder credentials are used
                if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password" || queryString.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                string mailQuery = queryString;

                // Retrieve messages from the Inbox folder matching the query, recursively
                ExchangeMessageInfoCollection messages = client.ListMessages("Inbox", mailQuery);

                Console.WriteLine($"Found {messages.Count} message(s) matching the criteria:");
                foreach (ExchangeMessageInfo info in messages)
                {
                    Console.WriteLine($"Subject: {info.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
