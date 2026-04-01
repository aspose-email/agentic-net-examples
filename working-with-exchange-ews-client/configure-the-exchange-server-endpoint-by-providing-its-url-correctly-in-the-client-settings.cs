using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

namespace ExchangeEndpointConfiguration
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder endpoint and credentials
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Guard against executing with placeholder values
                if (mailboxUri.Contains("example.com"))
                {
                    Console.WriteLine("Placeholder endpoint detected. Skipping connection.");
                    return;
                }

                // Create and configure the EWS client
                try
                {
                    using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                    {
                        // Example usage: display the Inbox URI
                        Console.WriteLine("Connected to Exchange server.");
                        Console.WriteLine("Inbox URI: " + client.MailboxInfo.InboxUri);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("EWS client error: " + ex.Message);
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unexpected error: " + ex.Message);
            }
        }
    }
}
