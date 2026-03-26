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
            // Mailbox URI and credentials for the service account
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("admin@example.com", "password");

            // Create the EWS client (implements IEWSClient)
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Impersonate another user (e.g., using SMTP address)
                client.ImpersonateUser(ItemChoice.SmtpAddress, "impersonated@example.com");

                // List messages in the impersonated user's Inbox
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);
                foreach (ExchangeMessageInfo info in messages)
                {
                    Console.WriteLine("Subject: " + info.Subject);
                    Console.WriteLine("From: " + info.From);
                    Console.WriteLine("Received: " + info.Date);
                    Console.WriteLine(new string('-', 40));
                }

                // Reset impersonation when done
                client.ResetImpersonation();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}