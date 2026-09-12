using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Define Exchange server URI and credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";

            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            NetworkCredential credentials = new NetworkCredential("username", "password", "DOMAIN");

            // Instantiate the Exchange service (EWS client) and establish a connection
            using (IEWSClient exchangeService = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Optional: configure additional properties
                exchangeService.LogFileName = "ews_log.txt";

                // Connection is established; you can now use exchangeService for further operations
                Console.WriteLine("Exchange service connected successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
