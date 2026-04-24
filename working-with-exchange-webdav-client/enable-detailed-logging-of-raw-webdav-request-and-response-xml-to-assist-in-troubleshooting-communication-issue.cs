using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.Dav;
using Aspose.Email.Clients.Exchange;

namespace AsposeEmailSample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder credentials – skip actual network call if they are not real.
                string mailboxUri = "https://exchange.example.com/ews/exchange.asmx";
                string username = "username";
                string password = "password";

                if (string.IsNullOrWhiteSpace(mailboxUri) ||
                    string.IsNullOrWhiteSpace(username) ||
                    string.IsNullOrWhiteSpace(password) ||
                    mailboxUri.Contains("example") ||
                    username.Contains("username") ||
                    password.Contains("password"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping Exchange server communication.");
                    return;
                }

                // Create ExchangeClient inside a using block to ensure proper disposal.
                using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
                {
                    // Enable detailed logging of raw WebDAV request/response XML.
                    client.LogFileName = "exchange_log.txt";

                    try
                    {
                        // List messages from the Inbox folder.
                        ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);
                        foreach (ExchangeMessageInfo messageInfo in messages)
                        {
                            Console.WriteLine($"Subject: {messageInfo.Subject}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error while listing messages: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
            }
        }
    }
}
