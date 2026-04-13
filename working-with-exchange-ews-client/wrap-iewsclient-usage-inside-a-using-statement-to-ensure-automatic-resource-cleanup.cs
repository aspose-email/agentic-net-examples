using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Define the EWS endpoint and user credentials
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (mailboxUri.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create the EWS client inside a using block for automatic disposal
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Retrieve mailbox information
                ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();

                // Display some useful folder URIs
                Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
                Console.WriteLine("Sent Items URI: " + mailboxInfo.SentItemsUri);
                Console.WriteLine("Drafts URI: " + mailboxInfo.DraftsUri);
                Console.WriteLine("Deleted Items URI: " + mailboxInfo.DeletedItemsUri);
            }
        }
        catch (Exception ex)
        {
            // Write any errors to the error stream
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
