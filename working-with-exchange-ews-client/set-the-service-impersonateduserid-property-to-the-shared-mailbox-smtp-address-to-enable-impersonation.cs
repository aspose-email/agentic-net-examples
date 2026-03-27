using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Mailbox URI and credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Impersonate the shared mailbox
                string sharedMailboxSmtp = "shared@example.com";
                client.ImpersonateUser(ItemChoice.SmtpAddress, sharedMailboxSmtp);

                Console.WriteLine("Impersonation set to: " + sharedMailboxSmtp);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
