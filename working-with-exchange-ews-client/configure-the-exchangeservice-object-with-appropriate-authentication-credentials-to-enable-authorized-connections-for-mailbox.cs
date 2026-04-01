using Aspose.Email.Clients.Exchange;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder credentials – in real scenarios replace with actual values.
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Skip actual network call when placeholders are detected.
            if (serviceUrl.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping connection.");
                return;
            }

            NetworkCredential credentials = new NetworkCredential(username, password);

            // Create the EWS client with authentication credentials.
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
                {
                    // Example operation: retrieve mailbox information.
                    try
                    {
                        ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();
                        Console.WriteLine("Inbox URI: " + mailboxInfo.InboxUri);
                        Console.WriteLine("Sent Items URI: " + mailboxInfo.SentItemsUri);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine("Error retrieving mailbox info: " + ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Failed to create or connect EWS client: " + ex.Message);
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
