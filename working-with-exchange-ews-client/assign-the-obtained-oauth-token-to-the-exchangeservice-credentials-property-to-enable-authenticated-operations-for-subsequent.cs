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
            // Input parameters (replace with real values)
            string mailboxUri = "https://outlook.office365.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string oauthToken = "your_oauth_token";


            // Skip external calls when placeholder credentials are used
            if (username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create the EWS client
            IEWSClient ewsClient = EWSClient.GetEWSClient(mailboxUri, username, password);

            // Assign the OAuth token to the Credentials property
            // NetworkCredential implements ICredentials and can hold the token as the user name.
            ewsClient.Credentials = new NetworkCredential(oauthToken, string.Empty);

            // Example operation: retrieve mailbox information
            var mailboxInfo = ewsClient.GetMailboxInfo();
            Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
