using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Author: Aspose.Email example – OAuth authentication for EWS
            // Define the EWS endpoint and the OAuth access token.
            string mailboxUri = "https://outlook.office365.com/EWS/Exchange.asmx";
            string oauthAccessToken = "YOUR_OAUTH_ACCESS_TOKEN";

            // Create credentials using the OAuth token.
            // If Aspose.Email provides a dedicated OAuth credentials class, replace this line accordingly.
            NetworkCredential oauthCredentials = new NetworkCredential("Bearer", oauthAccessToken);

            // Initialize the EWS client with the OAuth credentials.
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, oauthCredentials))
            {
                // Example operation: retrieve mailbox information.
                var mailboxInfo = client.GetMailboxInfo();
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
