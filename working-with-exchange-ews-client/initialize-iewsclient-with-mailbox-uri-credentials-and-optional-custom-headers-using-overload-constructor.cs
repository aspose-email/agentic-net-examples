using Aspose.Email;
using System;
using System.Net;
using System.Collections.Generic;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace AsposeEmailEwsSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define connection parameters
                string mailboxUri = "https://mail.example.com/EWS/Exchange.asmx";

                // Skip external calls when placeholder credentials are used
                if (mailboxUri.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                NetworkCredential credentials = new NetworkCredential("username", "password");
                WebProxy proxy = null; // No proxy; replace with actual proxy if needed

                // Optional custom headers
                Dictionary<string, string> customHeaders = new Dictionary<string, string>
                {
                    { "X-Custom-Header", "HeaderValue" }
                };

                // Initialize the EWS client with headers
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials, proxy, customHeaders))
                {
                    try
                    {
                        // Retrieve mailbox information
                        ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;

                        // Output some mailbox URIs
                        Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
                        Console.WriteLine("Sent Items URI: " + mailboxInfo.SentItemsUri);
                        Console.WriteLine("Drafts URI: " + mailboxInfo.DraftsUri);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Error while accessing mailbox info: " + ex.Message);
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
