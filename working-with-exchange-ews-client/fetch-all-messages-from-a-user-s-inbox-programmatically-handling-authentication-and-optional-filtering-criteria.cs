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
            // Initialize the EWS client with credentials
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(
                    "https://mail.example.com/EWS/Exchange.asmx",
                    new NetworkCredential("username", "password")))
                {
                    // Get mailbox information
                    ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;
                    string inboxUri = mailboxInfo.InboxUri;

                    // List messages in the Inbox folder
                    ExchangeMessageInfoCollection messagesInfo = client.ListMessages(inboxUri);

                    // Iterate through each message info and fetch the full message
                    foreach (ExchangeMessageInfo info in messagesInfo)
                    {
                        using (MailMessage message = client.FetchMessage(info.UniqueUri))
                        {
                            Console.WriteLine($"Subject: {message.Subject}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Client error: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
