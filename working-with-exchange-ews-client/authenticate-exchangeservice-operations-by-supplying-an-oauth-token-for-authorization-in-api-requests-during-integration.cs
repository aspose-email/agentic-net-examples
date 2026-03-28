using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace AsposeEmailOAuthExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder OAuth access token
                string accessToken = "YOUR_OAUTH_ACCESS_TOKEN";

                // EWS endpoint URL
                string ewsUrl = "https://outlook.office365.com/EWS/Exchange.asmx";

                // Create the EWS client using basic credentials (required by the factory)
                // The actual authentication will be performed via the OAuth token added as a header.
                using (IEWSClient client = EWSClient.GetEWSClient(ewsUrl, new NetworkCredential("user@example.com", "password")))
                {
                    // Add the OAuth Authorization header
                    client.AddHeader("Authorization", "Bearer " + accessToken);

                    // Retrieve mailbox information
                    ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();

                    // Output some mailbox URIs
                    Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
                    Console.WriteLine("Sent Items URI: " + mailboxInfo.SentItemsUri);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
