using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Mailbox URI and credentials for authentication
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Initialize EWS client with the provided credentials
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Optional: enable logging
                client.LogFileName = "ews_log.txt";

                // Client is ready for subsequent API operations
                Console.WriteLine("EWS client initialized successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
