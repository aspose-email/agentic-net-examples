using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Define connection parameters
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Create the EWS client inside a using block to ensure disposal
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
                {
                    // Retrieve mailbox information
                    ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();

                    // Output some mailbox URIs
                    Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
                    Console.WriteLine("Sent Items URI: " + mailboxInfo.SentItemsUri);

                    // List messages in the Inbox folder
                    ExchangeMessageInfoCollection messages = client.ListMessages(mailboxInfo.InboxUri);
                    foreach (ExchangeMessageInfo messageInfo in messages)
                    {
                        Console.WriteLine("Subject: " + messageInfo.Subject);
                        Console.WriteLine("From: " + messageInfo.From);
                        Console.WriteLine(new string('-', 40));
                    }
                }
            }
            catch (Exception ex)
            {
                // Write any unexpected errors to the error output
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
