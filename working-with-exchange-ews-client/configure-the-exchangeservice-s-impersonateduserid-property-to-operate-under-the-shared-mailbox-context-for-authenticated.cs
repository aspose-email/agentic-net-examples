using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Define connection parameters (replace with real values)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string sharedMailbox = "shared@example.com";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password" || sharedMailbox.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create EWS client
            IEWSClient ewsClient = EWSClient.GetEWSClient(mailboxUri, username, password);

            // Impersonate the shared mailbox
            ewsClient.ImpersonateUser(ItemChoice.PrimarySmtpAddress, sharedMailbox);

            // Example operation: retrieve mailbox information under impersonation
            var mailboxInfo = ewsClient.GetMailboxInfo();
            Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
            Console.WriteLine("Sent Items URI: " + mailboxInfo.SentItemsUri);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
            return;
        }
    }
}
