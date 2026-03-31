using System;
using System.Net;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping mailbox cleanup.");
                return;
            }

            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, new NetworkCredential(username, password)))
            {
                ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;
                ExchangeMessageInfoCollection inboxMessages = client.ListMessages(mailboxInfo.InboxUri);

                foreach (ExchangeMessageInfo messageInfo in inboxMessages)
                {
                    if (messageInfo.Subject != null && messageInfo.Subject.IndexOf("Unwanted", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        try
                        {
                            client.DeleteItem(messageInfo.UniqueUri, DeletionOptions.DeletePermanently);
                            Console.WriteLine($"Deleted message '{messageInfo.Subject}'.");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to delete message '{messageInfo.Subject}': {ex.Message}");
                        }
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
