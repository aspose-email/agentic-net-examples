using Aspose.Email;
using System;
using Aspose.Email.Clients.Exchange.Dav;
using System.Net;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection parameters
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip real network call when placeholders are detected
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping connection test.");
                return;
            }

            // Create the Exchange client
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                try
                {
                    // Attempt a lightweight operation to validate credentials
                    // Accessing mailbox info forces authentication without modifying data
                    client.GetMailboxInfo();
                    Console.WriteLine("Connection successful. Credentials are valid.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Connection failed: {ex.Message}");
                    // No rethrow; exit gracefully
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
