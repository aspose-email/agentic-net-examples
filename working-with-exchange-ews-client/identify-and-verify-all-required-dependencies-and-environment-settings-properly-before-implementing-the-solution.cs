using Aspose.Email.Storage.Pst;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email;
using System;
using System.Net;
using Aspose.Email.Clients.Exchange;

namespace AsposeEmailExchangeExample
{
    // Author: Aspose.Email example for IEWSClient usage
    class Program
    {
        static void Main()
        {
            // Exchange server connection parameters (replace with real values)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";
            string domain = "EXAMPLE";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            try
            {
                // Initialize the Exchange client with credentials
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password, domain))
                {
                    // Retrieve mailbox information
                    ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;

                    // Get information about the Inbox folder
                    ExchangeFolderInfo inboxInfo = client.GetFolderInfo(mailboxInfo.InboxUri);

                    // Output basic folder details
                    Console.WriteLine($"Inbox URI: {inboxInfo.Uri}");
                    Console.WriteLine($"Total Items in Inbox: {inboxInfo.TotalCount}");
                }
            }
            catch (Exception ex)
            {
                // Log any errors without crashing the application
                Console.Error.WriteLine($"Error accessing Exchange server: {ex.Message}");
            }
        }
    }
}
