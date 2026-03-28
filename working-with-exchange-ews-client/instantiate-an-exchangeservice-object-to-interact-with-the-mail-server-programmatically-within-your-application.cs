using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        // Top‑level exception guard
        try
        {
            // Connection parameters (replace with real values)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create the EWS client (disposed automatically)
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Retrieve mailbox information
                ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();

                Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
                Console.WriteLine("Sent Items URI: " + mailboxInfo.SentItemsUri);
            }
        }
        catch (Exception ex)
        {
            // Friendly error output
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
