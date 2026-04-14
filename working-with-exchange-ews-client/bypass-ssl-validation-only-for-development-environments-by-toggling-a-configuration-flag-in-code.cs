using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Configuration flag to bypass SSL validation in development environments
            bool ignoreSslValidation = true;
            if (ignoreSslValidation)
            {
                ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            }

            // Exchange server connection settings
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create the EWS client with connection safety handling
            IEWSClient client = null;
            try
            {
                client = EWSClient.GetEWSClient(mailboxUri, username, password);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Failed to create EWS client: " + ex.Message);
                return;
            }

            // Use the client and ensure proper disposal
            using (client)
            {
                try
                {
                    // Retrieve mailbox information (avoid using non‑existent DisplayName property)
                    ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;

                    // Output a sample property to verify the connection
                    Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error accessing mailbox information: " + ex.Message);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
