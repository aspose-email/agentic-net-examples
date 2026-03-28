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
            // Initialize the EWS client with server URL and credentials.
            using (IEWSClient client = EWSClient.GetEWSClient(
                "https://exchange.example.com/EWS/Exchange.asmx",
                new NetworkCredential("username", "password")))
            {
                // Retrieve mailbox information.
                ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();
                Console.WriteLine($"Inbox URI: {mailboxInfo.InboxUri}");

                // List messages in the Inbox folder.
                ExchangeMessageInfoCollection messages = client.ListMessages(mailboxInfo.InboxUri);
                foreach (ExchangeMessageInfo messageInfo in messages)
                {
                    Console.WriteLine($"Subject: {messageInfo.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
