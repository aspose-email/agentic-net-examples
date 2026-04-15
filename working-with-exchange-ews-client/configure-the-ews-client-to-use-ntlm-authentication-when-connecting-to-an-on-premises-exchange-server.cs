using Aspose.Email;
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
            // Define connection parameters
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user";
            string password = "password";
            string domain = "DOMAIN";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create NTLM credentials
            NetworkCredential credentials = new NetworkCredential(username, password, domain);

            // Initialize EWS client with NTLM authentication
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Optionally, verify the client is connected by retrieving mailbox info
                ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();

                // Display a mailbox folder URI to confirm successful connection
                Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
