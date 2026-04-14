using System.Net;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Base;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Exchange server connection details
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create EWS client and configure TLS encryption
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Enforce TLS 1.2 (adjust as needed)
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                try
                {
                    // Verify handshake by retrieving mailbox information
                    ExchangeMailboxInfo info = client.GetMailboxInfo();
                    Console.WriteLine("Successfully connected. Mailbox: " + info.MailboxUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Handshake failed: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
