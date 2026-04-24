using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Clients.Exchange;

namespace AsposeEmailWebDavExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define connection parameters (replace with real values for actual use)
                string mailboxUri = "https://example.com/exchange";
                string username = "username";
                string password = "password";

                // Detect placeholder credentials and skip actual network call
                if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping connection to Exchange server.");
                    return;
                }

                // Establish a WebDAV connection to the Exchange server
                using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
                {
                    try
                    {
                        // Attempt to list messages from the Inbox folder
                        ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);

                        // Iterate through the messages and display basic information
                        foreach (ExchangeMessageInfo messageInfo in messages)
                        {
                            // Use the available Date property (ReceivedDate does not exist)
                            Console.WriteLine("Subject: {0}", messageInfo.Subject);
                            Console.WriteLine("Date: {0}", messageInfo.InternalDate);
                            Console.WriteLine("From: {0}", messageInfo.From);
                            Console.WriteLine(new string('-', 40));
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Error while accessing mailbox: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unhandled exception: " + ex.Message);
            }
        }
    }
}
