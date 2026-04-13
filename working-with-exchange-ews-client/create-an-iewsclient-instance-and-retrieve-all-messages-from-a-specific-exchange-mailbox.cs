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
            // Define connection parameters
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create IEWSClient instance
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Get the Inbox folder URI
                string inboxUri = client.MailboxInfo.InboxUri;

                // List all messages in the Inbox
                ExchangeMessageInfoCollection messagesInfo = client.ListMessages(inboxUri);

                // Iterate through each message info and fetch the full message
                foreach (var messageInfo in messagesInfo)
                {
                    var mailMessage = client.FetchMessage(messageInfo.UniqueUri);
                    Console.WriteLine($"Subject: {mailMessage.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
