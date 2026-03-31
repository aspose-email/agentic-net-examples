using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder credentials – replace with real values when needed.
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "username";
                string password = "password";

                // Detect placeholder values and skip actual network call.
                if (mailboxUri.Contains("example.com") || username == "username" || password == "password")
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping connection to Exchange server.");
                    return;
                }

                // Create the EWS client with authentication credentials.
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    try
                    {
                        // Retrieve mailbox information to verify authentication.
                        ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();
                        Console.WriteLine($"Inbox URI: {mailboxInfo.InboxUri}");
                        Console.WriteLine($"Sent Items URI: {mailboxInfo.SentItemsUri}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error while accessing mailbox: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
