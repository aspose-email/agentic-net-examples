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
            // Initialize the EWS client using the factory method.
            // Replace placeholder values with actual server URL and credentials.
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, new NetworkCredential(username, password)))
            {
                try
                {
                    // Retrieve mailbox information.
                    ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();

                    // List messages in the Inbox folder.
                    ExchangeMessageInfoCollection messages = client.ListMessages(mailboxInfo.InboxUri);

                    Console.WriteLine("Total messages in Inbox: " + messages.Count);

                    if (messages.Count > 0)
                    {
                        // Fetch the first message using its UniqueUri.
                        MailMessage firstMessage = client.FetchMessage(messages[0].UniqueUri);
                        Console.WriteLine("Subject of first message: " + firstMessage.Subject);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("EWS operation failed: " + ex.Message);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
            return;
        }
    }
}
