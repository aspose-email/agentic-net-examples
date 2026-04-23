using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values when running against an actual server.
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against executing network calls with placeholder data.
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder mailbox URI detected. Skipping Exchange client operations.");
                return;
            }

            // Create the Exchange client and set a higher timeout (e.g., 5 minutes).
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                try
                {
                    client.Timeout = 300000; // Timeout in milliseconds (5 minutes)
                    Console.WriteLine($"Exchange client timeout set to {client.Timeout} ms.");
                    
                    // Additional operations can be performed here.
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error configuring Exchange client: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
