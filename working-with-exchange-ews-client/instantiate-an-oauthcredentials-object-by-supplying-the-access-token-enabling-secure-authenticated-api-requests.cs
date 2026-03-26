using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Access token for OAuth authentication
            string accessToken = "your_access_token";

            // EWS endpoint URL
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";

            // Create OAuth credential (NetworkCredential implementation)
            Aspose.Email.Clients.OAuthNetworkCredential oauthCredential = new Aspose.Email.Clients.OAuthNetworkCredential(accessToken);

            // Initialize EWS client with OAuth credentials
            using (Aspose.Email.Clients.Exchange.WebService.IEWSClient ewsClient = Aspose.Email.Clients.Exchange.WebService.EWSClient.GetEWSClient(mailboxUri, oauthCredential))
            {
                // Retrieve mailbox information (e.g., Inbox URI)
                Aspose.Email.Clients.Exchange.ExchangeMailboxInfo mailboxInfo = ewsClient.MailboxInfo;
                Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}