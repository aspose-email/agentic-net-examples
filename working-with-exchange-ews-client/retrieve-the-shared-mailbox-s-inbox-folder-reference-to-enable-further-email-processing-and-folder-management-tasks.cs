using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define the EWS endpoint and credentials for the shared mailbox
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password", "domain");

            // Create the EWS client using the factory method
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Retrieve the Inbox folder URI of the shared mailbox
                string inboxFolderUri = client.MailboxInfo.InboxUri;
                Console.WriteLine("Inbox folder URI: " + inboxFolderUri);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
