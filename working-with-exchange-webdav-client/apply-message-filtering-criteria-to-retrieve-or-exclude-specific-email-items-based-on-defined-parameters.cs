using Aspose.Email.Tools.Search;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Clients.Exchange;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Initialize the Exchange WebDAV client
                try
                {
                    using (ExchangeClient client = new ExchangeClient("https://exchange.example.com/EWS/Exchange.asmx", "username", "password"))
                    {
                        // Build a query to filter messages (e.g., subject contains "Invoice")
                        ExchangeQueryBuilder builder = new ExchangeQueryBuilder();
                        builder.Subject.Contains("Invoice");
                        MailQuery query = builder.GetQuery();

                        // Retrieve messages from the Inbox folder that match the query
                        ExchangeMessageInfoCollection messages = client.ListMessages("Inbox", query, false);

                        // Process and display the filtered messages
                        foreach (ExchangeMessageInfo info in messages)
                        {
                            Console.WriteLine($"Subject: {info.Subject}");
                            Console.WriteLine($"Date: {info.Date}");
                            Console.WriteLine($"From: {info.From}");
                            Console.WriteLine(new string('-', 40));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Client error: {ex.Message}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
