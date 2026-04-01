using System;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        // Top‑level exception guard
        try
        {
            // Placeholder connection details
            string host = "exchange.example.com";
            string username = "user@example.com";
            string password = "password";

            // Skip real network call when placeholders are used
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping server connection.");
                return;
            }

            // Ensure the client is instantiated before any use
            using (ExchangeClient client = new ExchangeClient(host, username, password))
            {
                // Guard client operations for connection/authentication errors
                try
                {
                    // Retrieve messages from the Inbox folder
                    ExchangeMessageInfoCollection messages = client.ListMessages("Inbox");

                    // Apply filtering criteria
                    foreach (ExchangeMessageInfo info in messages)
                    {
                        // Example criteria: subject contains "Invoice" and size > 10 KB
                        if (info.Subject != null &&
                            info.Subject.Contains("Invoice") &&
                            info.Size > 10_000)
                        {
                            Console.WriteLine($"Subject: {info.Subject}");
                            Console.WriteLine($"Size: {info.Size} bytes");
                            Console.WriteLine($"From: {info.From}");
                            Console.WriteLine($"Received: {info.Date}");
                            Console.WriteLine(new string('-', 40));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Exchange operation failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
