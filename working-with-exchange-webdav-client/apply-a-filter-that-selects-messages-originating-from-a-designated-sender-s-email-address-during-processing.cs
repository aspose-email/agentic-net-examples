using Aspose.Email.Clients.Exchange;
using System;
using Aspose.Email;
using Aspose.Email.Tools.Search;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize Exchange WebDAV client (placeholder credentials)
            string exchangeUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials
            if (exchangeUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network call.");
                return;
            }

            using (ExchangeClient client = new ExchangeClient(exchangeUrl, username, password))
            {
                // Build a query to filter messages from a specific sender
                string senderEmail = "sender@example.com";
                MailQuery query = new MailQuery($"('From' = '{senderEmail}')");

                // List messages in the Inbox that match the query (recursive set to false)
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, query, false);

                // Process the filtered messages
                foreach (ExchangeMessageInfo info in messages)
                {
                    Console.WriteLine($"Subject: {info.Subject}");
                    Console.WriteLine($"From: {info.From}");
                    Console.WriteLine($"Date: {info.InternalDate}");
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
