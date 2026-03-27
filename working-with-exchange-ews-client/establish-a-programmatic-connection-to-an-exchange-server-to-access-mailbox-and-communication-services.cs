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
            // Connection parameters (replace with real values)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create EWS client safely
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // Get Inbox folder URI
                    string inboxUri = client.MailboxInfo.InboxUri;

                    // List messages in the Inbox
                    ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri);

                    foreach (ExchangeMessageInfo info in messages)
                    {
                        // Fetch full message for each item
                        using (MailMessage message = client.FetchMessage(info.UniqueUri))
                        {
                            Console.WriteLine($"Subject: {message.Subject}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Mailbox operation failed: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled error: {ex.Message}");
        }
    }
}
