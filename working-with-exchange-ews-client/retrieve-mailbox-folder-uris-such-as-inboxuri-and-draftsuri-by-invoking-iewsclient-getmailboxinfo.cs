using Aspose.Email.Clients.Exchange;
using Aspose.Email;
using System;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define connection parameters
                string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create the EWS client
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // Retrieve mailbox information
                    ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;

                    // Get folder URIs
                    string inboxUri = mailboxInfo.InboxUri;
                    string draftsUri = mailboxInfo.DraftsUri;

                    // Output the URIs
                    Console.WriteLine($"Inbox URI: {inboxUri}");
                    Console.WriteLine($"Drafts URI: {draftsUri}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
