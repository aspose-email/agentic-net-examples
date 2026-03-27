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
            // Define the EWS service URL and user credentials (placeholders)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password", "DOMAIN");

            // Create the EWS client instance
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                try
                {
                    // Impersonate the target user
                    string impersonatedUser = "impersonated@example.com";
                    client.ImpersonateUser(ItemChoice.SmtpAddress, impersonatedUser);

                    // Retrieve messages from the impersonated user's Inbox
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);
                    foreach (ExchangeMessageInfo info in messages)
                    {
                        // Fetch the full message to access its properties
                        MailMessage message = client.FetchMessage(info.UniqueUri);
                        Console.WriteLine("Subject: " + message.Subject);
                        // Dispose the fetched message
                        message.Dispose();
                    }

                    // Reset impersonation after operations are complete
                    client.ResetImpersonation();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error during impersonated operations: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Fatal error: " + ex.Message);
        }
    }
}