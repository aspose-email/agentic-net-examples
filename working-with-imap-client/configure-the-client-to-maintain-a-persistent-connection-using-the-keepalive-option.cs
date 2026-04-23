using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – skip real network calls in sample environments
            string host = "exchange.example.com";
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder host detected. Skipping connection to Exchange server.");
                return;
            }

            // Create and configure the Exchange client with persistent connection
            try
            {
                using (ExchangeClient client = new ExchangeClient(host, username, password))
                {
                    // Enable KeepAlive to maintain a persistent connection
                    client.KeepAlive = true;

                    // Additional optional settings can be configured here
                    // client.PreAuthenticate = true;

                    Console.WriteLine("Exchange client configured with KeepAlive = true.");
                    // Perform further operations as needed, e.g., client.ListMessages("INBOX");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create or configure Exchange client: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
