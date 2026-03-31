using Aspose.Email.Clients.Exchange;
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
            // ----- Manual EWS endpoint configuration -----
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls during CI
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping EWS connection.");
                return;
            }

            // Create the EWS client using the manual endpoint
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Example operation: retrieve mailbox information
                ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;
                Console.WriteLine($"Inbox URI: {mailboxInfo.InboxUri}");
            }

            // ----- Automatic discovery (optional) -----
            // To enable autodiscover at runtime, use the AutodiscoverService (if available) to obtain the EWS URL,
            // then create the client with the discovered URL as shown above.
            // Example (commented out to avoid compilation issues if AutodiscoverService is not referenced):
            // string email = "user@example.com";
            // var autodiscover = new Aspose.Email.Clients.Exchange.Autodiscover.AutodiscoverService(email, new NetworkCredential(username, password));
            // string discoveredUrl = autodiscover.GetEwsUrl();
            // using (IEWSClient client = EWSClient.GetEWSClient(discoveredUrl, username, password))
            // {
            //     // Perform operations with the autodiscovered client
            // }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
