using Aspose.Email.Clients.Exchange;
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
            // Exchange server URL and credentials
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string adminUsername = "admin@example.com";
            string adminPassword = "adminPassword";
            string domain = "example.com";
            string impersonatedMailbox = "user@example.com";

            // Create EWS client with admin credentials
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, new NetworkCredential(adminUsername, adminPassword, domain)))
            {
                try
                {
                    // Impersonate the target mailbox
                    client.MailboxUri = impersonatedMailbox;

                    // Example operation: list subjects of messages in the Inbox
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);
                    foreach (ExchangeMessageInfo messageInfo in messages)
                    {
                        Console.WriteLine(messageInfo.Subject);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during impersonated operations: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
