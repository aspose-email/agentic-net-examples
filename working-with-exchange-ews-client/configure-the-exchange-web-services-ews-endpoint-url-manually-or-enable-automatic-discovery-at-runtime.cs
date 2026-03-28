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
            // Manual configuration of the EWS endpoint URL
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Example operation: list messages in the Inbox folder
                foreach (var messageInfo in client.ListMessages(client.MailboxInfo.InboxUri))
                {
                    Console.WriteLine(messageInfo.UniqueUri);
                }
            }

            // To enable automatic discovery at runtime, you can obtain the service URL
            // via Aspose.Email's AutodiscoverService (not shown here) and then create the client:
            // string discoveredUrl = AutodiscoverService.GetEwsUrl(email, credentials);
            // using (IEWSClient client = EWSClient.GetEWSClient(discoveredUrl, credentials)) { ... }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
