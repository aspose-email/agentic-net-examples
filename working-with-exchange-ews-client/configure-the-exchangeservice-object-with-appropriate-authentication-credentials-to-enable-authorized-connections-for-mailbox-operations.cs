using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main(string[] args)
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Mailbox URI of the Exchange server
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";

            // Authentication credentials
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create and configure the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Example: output the Inbox URI to verify connection
                Console.WriteLine("Connected. Inbox URI: " + client.MailboxInfo.InboxUri);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}