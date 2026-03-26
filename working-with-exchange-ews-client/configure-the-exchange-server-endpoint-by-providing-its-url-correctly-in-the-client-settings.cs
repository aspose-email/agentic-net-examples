using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Define the Exchange Web Services (EWS) endpoint and credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create the EWS client. The factory returns an IEWSClient implementation.
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Retrieve mailbox information and display key folder URIs
                ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;
                Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
                Console.WriteLine("Sent Items URI: " + mailboxInfo.SentItemsUri);
            }
        }
        catch (Exception ex)
        {
            // Output any errors to the error stream without crashing the application
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}