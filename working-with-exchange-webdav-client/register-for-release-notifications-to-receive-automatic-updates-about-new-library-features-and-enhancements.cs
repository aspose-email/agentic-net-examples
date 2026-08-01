using Aspose.Email;
using System;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        // Placeholder values – replace with real credentials to run against an actual server.
        string mailboxUri = "https://exchange.example.com/exchange/username";
        string username = "username";
        string password = "password";

        // Skip external calls when placeholder credentials are used.
        if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
        {
            Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
            return;
        }

        try
        {
            // ExchangeClient must be instantiated with the three‑parameter constructor inside a using block.
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                Console.WriteLine("Connected to Exchange WebDAV successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error connecting to Exchange: {ex.Message}");
        }
    }
}
