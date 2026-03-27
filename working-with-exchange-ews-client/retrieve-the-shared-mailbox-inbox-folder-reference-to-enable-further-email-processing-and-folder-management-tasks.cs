using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client for the shared mailbox
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "shared_mailbox_user";
            string password = "password";

            // Create the client (factory returns an IEWSClient)
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
            {
                // Retrieve mailbox information
                ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;

                // Get the Inbox folder URI
                string inboxFolderUri = mailboxInfo.InboxUri;

                // Output the Inbox folder reference
                Console.WriteLine("Inbox folder URI: " + inboxFolderUri);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
