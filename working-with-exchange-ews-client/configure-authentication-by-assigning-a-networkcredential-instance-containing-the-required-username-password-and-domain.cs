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
            // Author note: This sample demonstrates configuring authentication for EWS using NetworkCredential.
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create a NetworkCredential with username, password, and domain.
            NetworkCredential credentials = new NetworkCredential("username", "password", "DOMAIN");

            // Initialize the EWS client with the mailbox URI and credentials.
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                try
                {
                    // Example operation: retrieve mailbox information.
                    var mailboxInfo = client.GetMailboxInfo();
                    Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("EWS operation failed: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
