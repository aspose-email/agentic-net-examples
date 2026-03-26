using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

namespace ExchangeServiceExample
{
    class Program
    {
        static void Main(string[] args)
        {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

            try
            {
                // Define the Exchange Web Services (EWS) mailbox URI
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";

                // Create network credentials for authentication
                NetworkCredential credentials = new NetworkCredential("username", "password");

                // Instantiate the EWS client using the factory method
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
                {
                    // Configure session settings
                    client.Timeout = 120000; // Timeout in milliseconds
                    client.UseDateInLogFileName = true;

                    // Example: output the mailbox URI to verify the client is configured
                    Console.WriteLine("EWS client initialized for mailbox: " + client.MailboxInfo.MailboxUri);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
                return;
            }
        }
    }
}