using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder connection settings
            string host = "exchange.example.com";
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create and use the Exchange WebDAV client
            using (ExchangeClient client = new ExchangeClient(host, new NetworkCredential(username, password)))
            {
                try
                {
                    // List messages in the Inbox folder
                    string inboxUri = client.MailboxInfo.InboxUri;
                    ExchangeMessageInfoCollection messageInfos = client.ListMessages(inboxUri);

                    foreach (ExchangeMessageInfo messageInfo in messageInfos)
                    {
                        // Fetch each message
                        using (MailMessage mail = client.FetchMessage(messageInfo.UniqueUri))
                        {
                            // Output subject and plain‑text body
                            Console.WriteLine("Subject: " + mail.Subject);
                            Console.WriteLine("Body: " + mail.Body);
                            Console.WriteLine(new string('-', 40));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error during message retrieval: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unhandled exception: " + ex.Message);
        }
    }
}
