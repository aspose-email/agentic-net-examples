using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Tools.Search;

namespace AsposeEmailWebDavFilterExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder connection settings – replace with real values.
                string serverUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "username";
                string password = "password";

                // Guard against executing with placeholder credentials.
                if (serverUri.Contains("example.com") || username == "username" || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                    return;
                }

                // Create and connect the Exchange WebDAV client.
                try
                {
                    using (ExchangeClient client = new ExchangeClient(serverUri, username, password))
                    {
                        // Build a query to retrieve only unread messages that have attachments.
                        // The query language follows the Exchange MailQuery syntax.
                        MailQuery query = new MailQuery("HasAttachment = True AND IsRead = False");

                        // List messages from the Inbox that match the query (recursive = false).
                        ExchangeMessageInfoCollection messages = client.ListMessages(
                            client.MailboxInfo.InboxUri,
                            query,
                            false);

                        // Process the filtered messages.
                        foreach (ExchangeMessageInfo info in messages)
                        {
                            Console.WriteLine("Subject: " + info.Subject);
                            Console.WriteLine("Has Attachments: " + info.HasAttachments);
                            Console.WriteLine("Received: " + info.InternalDate);
                            Console.WriteLine(new string('-', 40));
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Error during client operation: " + ex.Message);
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unhandled exception: " + ex.Message);
                return;
            }
        }
    }
}
