using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Replace with your actual mailbox URI and credentials
            string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Get the Inbox folder URI
                string inboxUri = client.MailboxInfo.InboxUri;

                // List all messages in the Inbox
                ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri);

                // Iterate and output subjects of unread messages
                foreach (ExchangeMessageInfo messageInfo in messages)
                {
                    if (!messageInfo.IsRead)
                    {
                        Console.WriteLine(messageInfo.Subject);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Output any errors to the error stream
            Console.Error.WriteLine(ex.Message);
        }
    }
}
