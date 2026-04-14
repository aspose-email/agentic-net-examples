using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            // Top‑level exception guard
            try
            {
                // Exchange server autodiscover URL and user credentials
                string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create the EWS client (wrapped in using for disposal)
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // Wrap client operations in a try/catch to handle connection/authentication errors
                    try
                    {
                        // Retrieve mailbox information
                        ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;

                        // Output some useful URIs from the mailbox info
                        Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
                        Console.WriteLine("Sent Items URI: " + mailboxInfo.SentItemsUri);
                        Console.WriteLine("Drafts URI: " + mailboxInfo.DraftsUri);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Error accessing mailbox information: " + ex.Message);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unexpected error: " + ex.Message);
            }
        }
    }
}
