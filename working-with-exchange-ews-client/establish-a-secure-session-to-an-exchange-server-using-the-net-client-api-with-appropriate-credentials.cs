using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Connection parameters – replace with real values.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create network credentials.
            NetworkCredential credentials = new NetworkCredential(username, password);
            // If a domain is required, use: new NetworkCredential(username, password, "DOMAIN");

            // Initialize the EWS client (secure session).
            IEWSClient ewsClient = EWSClient.GetEWSClient(mailboxUri, credentials);

            // Example operation: retrieve mailbox information.
            var mailboxInfo = ewsClient.GetMailboxInfo();
            Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
        }
        catch (ExchangeException ex)
        {
            Console.Error.WriteLine("Exchange error: " + ex.Message);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
