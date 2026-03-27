using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Replace with actual mailbox URI and credentials
                string mailboxUri = "https://exchange.example.com/exchange/user@domain.com/";
                string username = "user@domain.com";
                string password = "password";

                // Create and dispose the Exchange WebDAV client safely
                using (Aspose.Email.Clients.Exchange.Dav.ExchangeClient client = new Aspose.Email.Clients.Exchange.Dav.ExchangeClient(mailboxUri, username, password))
                {
                    // Get the Inbox folder URI from the mailbox info
                    string inboxUri = client.MailboxInfo.InboxUri;

                    // List messages in the Inbox folder
                    Aspose.Email.Clients.Exchange.ExchangeMessageInfoCollection messages = client.ListMessages(inboxUri);

                    // Enumerate and display basic metadata for each message
                    foreach (Aspose.Email.Clients.Exchange.ExchangeMessageInfo info in messages)
                    {
                        Console.WriteLine("Subject: " + info.Subject);
                        Console.WriteLine("From: " + info.From);
                        Console.WriteLine("Date: " + info.Date);
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
            catch (Exception ex)
            {
                // Output any errors without crashing the application
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}