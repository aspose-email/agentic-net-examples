using System;
using System.Collections.Generic;
using System.Linq;
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
                // Placeholder values – replace with real server details.
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "username";
                string password = "password";

                // Skip execution when placeholder credentials are detected.
                if (mailboxUri.Contains("example.com") || username == "username")
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping Exchange connection.");
                    return;
                }

                // Create and use the Exchange WebDAV client.
                using (ExchangeClient client = new ExchangeClient(mailboxUri, username, password))
                {
                    // Retrieve messages from the Inbox folder.
                    ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);

                    // Sort messages by the internal received date in descending order.
                    List<ExchangeMessageInfo> sortedMessages = messages
                        .OrderByDescending(msg => msg.InternalDate)
                        .ToList();

                    // Display subject and received date for each message.
                    foreach (ExchangeMessageInfo info in sortedMessages)
                    {
                        Console.WriteLine($"{info.Subject} - {info.InternalDate}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine(ex.Message);
            }
        }
    }
}
