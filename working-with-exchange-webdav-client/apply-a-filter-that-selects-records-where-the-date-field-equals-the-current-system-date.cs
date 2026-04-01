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
            // Placeholder credentials – skip execution to avoid real network calls
            string exchangeUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            if (exchangeUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network operation.");
                return;
            }

            // Create Exchange client inside a try/catch to handle connection issues
            try
            {
                using (ExchangeClient client = new ExchangeClient(exchangeUri, username, password))
                {
                    // Build a query that selects messages where the internal date equals today
                    MailQueryBuilder builder = new MailQueryBuilder();
                    MailQuery query = builder.InternalDate.On(DateTime.Today);

                    // List messages from the Inbox that match the query
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri, query, true);

                    foreach (ExchangeMessageInfo info in messages)
                    {
                        Console.WriteLine($"Subject: {info.Subject}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error connecting to Exchange: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
