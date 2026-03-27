using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // EWS service URL and credentials
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string adminUser = "admin@example.com";
            string adminPassword = "password";
            NetworkCredential credentials = new NetworkCredential(adminUser, adminPassword);

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
            {
                // Impersonate another user
                client.ImpersonateUser(ItemChoice.PrimarySmtpAddress, "impersonated@example.com");

                // List messages in the impersonated user's Inbox
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);
                foreach (ExchangeMessageInfo info in messages)
                {
                    Console.WriteLine($"Subject: {info.Subject}");
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
