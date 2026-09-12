using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        // Exchange WebDAV service URL (e.g., https://mail.example.com/ews/exchange.asmx)
        string serviceUrl = "https://exchange.example.com/ews/exchange.asmx";

        // Credentials of an account that has permission to access other mailboxes
        string adminUsername = "admin@example.com";
        string adminPassword = "adminPassword";
        string domain = "example.com";

        // The mailbox of the user whose messages we want to read
        string targetMailbox = "user@example.com";

        try
        {
            // Initialize the Exchange client with admin credentials
            using (ExchangeClient client = new ExchangeClient(serviceUrl, adminUsername, adminPassword, domain))
            {
                // Retrieve mailbox information for the target user
                ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo(targetMailbox);

                // List messages in the target user's Inbox folder
                ExchangeMessageInfoCollection messages = client.ListMessages(mailboxInfo.InboxUri);

                // Iterate through each message and fetch its full content
                foreach (ExchangeMessageInfo msgInfo in messages)
                {
                    MailMessage message = client.FetchMessage(msgInfo.UniqueUri);
                    Console.WriteLine($"Subject: {message.Subject}");
                    Console.WriteLine($"From: {message.From}");
                    Console.WriteLine($"To: {string.Join(", ", message.To)}");
                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
