using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client with placeholder credentials
            using (IEWSClient client = EWSClient.GetEWSClient(
                "https://exchange.example.com/EWS/Exchange.asmx",
                new NetworkCredential("username", "password")))
            {
                // Retrieve mailbox information
                ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;

                // List messages in the Inbox folder
                ExchangeMessageInfoCollection messages = client.ListMessages(mailboxInfo.InboxUri);

                foreach (ExchangeMessageInfo info in messages)
                {
                    // Fetch full message using its unique URI
                    using (MailMessage message = client.FetchMessage(info.UniqueUri))
                    {
                        Console.WriteLine($"Subject: {message.Subject}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
