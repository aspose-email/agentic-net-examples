using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Exchange server connection details (replace with real values)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create and connect the Exchange WebDAV client
            using (ExchangeClient client = new ExchangeClient(mailboxUri, new NetworkCredential(username, password)))
            {
                // Build a query to retrieve only unread messages
                MailQuery unreadQuery = new MailQuery("IsRead = 'False'");

                // List unread messages from the Inbox folder
                ExchangeMessageInfoCollection messages = client.ListMessages("Inbox", unreadQuery, false);

                Console.WriteLine($"Found {messages.Count} unread message(s):");
                foreach (ExchangeMessageInfo info in messages)
                {
                    // Fetch the full message to access its properties
                    MailMessage message = client.FetchMessage(info.UniqueUri);
                    Console.WriteLine($"Subject: {message.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
