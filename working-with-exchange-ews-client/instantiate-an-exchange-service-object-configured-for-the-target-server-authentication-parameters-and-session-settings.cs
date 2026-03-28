using System;
using System.Net;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Initialize the EWS client with mailbox URI and credentials
            using (IEWSClient client = EWSClient.GetEWSClient(
                "https://exchange.example.com/EWS/Exchange.asmx",
                new NetworkCredential("username", "password")))
            {
                // Configure session settings
                client.Timeout = 120000; // Timeout in milliseconds
                client.UseDateInLogFileName = true;
                client.LogFileName = "ews_log.txt";

                // Example: set a proxy if required (commented out as placeholder)
                // client.Proxy = new WebProxy("http://proxy.example.com:8080");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
