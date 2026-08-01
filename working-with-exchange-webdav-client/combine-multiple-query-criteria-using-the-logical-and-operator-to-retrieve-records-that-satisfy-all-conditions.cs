using Aspose.Email.Clients.Exchange;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Tools.Search;

namespace AsposeEmailExchangeQuerySample
{
    class Program
    {
        static void Main()
        {
            // Define connection parameters (replace with real values)
            string mailboxUri = "https://exchange.example.com";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Build a query that combines multiple criteria with logical AND
            // Example: messages from a specific sender AND with a subject containing a keyword
            MailQuery query = new MailQuery("(('From' Contains 'john@example.com') & 'Subject' Contains 'Invoice')");

            try
            {
                // Instantiate ExchangeClient inside a using block as required
                using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
                {
                    // Retrieve messages from the Inbox folder that match the query
                    ExchangeMessageInfoCollection messages = client.ListMessages("Inbox", query.ToString());

                    // Iterate over the results and display basic information
                    foreach (ExchangeMessageInfo info in messages)
                    {
                        Console.WriteLine($"Subject: {info.Subject}");
                        Console.WriteLine($"From: {info.From}");
                        // Use InternalDate as the supported date property
                        Console.WriteLine($"Received: {info.InternalDate}");
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
            catch (Exception ex)
            {
                // Log any errors without throwing
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
