using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

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

            // Create and connect the EWS client inside a using block for proper disposal
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // Optionally retrieve server version to verify connection
                    string versionInfo = client.GetVersionInfo();
                    Console.WriteLine("Exchange Server Version: " + versionInfo);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to retrieve server version: " + ex.Message);
                    return;
                }

                try
                {
                    // Get mailbox information
                    ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();

                    // Display some useful mailbox URIs
                    Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
                    Console.WriteLine("Sent Items URI: " + mailboxInfo.SentItemsUri);
                    Console.WriteLine("Drafts URI: " + mailboxInfo.DraftsUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to get mailbox information: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("An unexpected error occurred: " + ex.Message);
        }
    }
}
