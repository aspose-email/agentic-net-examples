using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Ensure TLS 1.2 is used for the connection
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            // Define connection parameters
            string mailboxUri = "https://legacy.exchange.server/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create the EWS client inside a using block to guarantee disposal
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // Retrieve mailbox information
                    ExchangeMailboxInfo mailboxInfo = client.MailboxInfo;

                    // Output some useful URIs to verify the connection
                    Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
                    Console.WriteLine("Sent Items URI: " + mailboxInfo.SentItemsUri);
                    Console.WriteLine("Drafts URI: " + mailboxInfo.DraftsUri);
                }
                catch (Exception ex)
                {
                    // Handle errors related to mailbox operations
                    Console.Error.WriteLine("Error accessing mailbox: " + ex.Message);
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            // Top‑level error handling
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
