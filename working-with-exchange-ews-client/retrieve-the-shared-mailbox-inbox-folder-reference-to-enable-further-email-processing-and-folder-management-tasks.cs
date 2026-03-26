using System.Net;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // EWS service URL and credentials
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Email address of the shared mailbox
            string sharedMailbox = "shared@example.com";

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Retrieve mailbox information for the shared mailbox
                ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo(sharedMailbox);

                // Get the Inbox folder URI
                string inboxUri = mailboxInfo.InboxUri;

                Console.WriteLine("Shared mailbox Inbox URI: " + inboxUri);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}