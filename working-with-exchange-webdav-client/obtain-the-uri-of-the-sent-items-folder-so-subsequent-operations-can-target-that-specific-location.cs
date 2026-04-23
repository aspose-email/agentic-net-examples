using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Define connection parameters
            string mailboxUri = "https://exchange.example.com/ews/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard against placeholder credentials to avoid live network calls
            if (mailboxUri.Contains("example") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Exchange connection.");
                return;
            }

            // Create and use the Exchange client
            using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
            {
                try
                {
                    // Obtain the Sent Items folder URI
                    string sentItemsUri = client.MailboxInfo.SentItemsUri;
                    Console.WriteLine($"Sent Items folder URI: {sentItemsUri}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to retrieve Sent Items URI: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
