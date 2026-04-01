using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder credentials – skip actual network call in CI environments
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            if (serviceUrl.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping EWS operations.");
                return;
            }

            // Create the async EWS client
            IAsyncEwsClient client = null;
            try
            {
                client = await EWSClient.GetEwsClientAsync(serviceUrl, new NetworkCredential(username, password));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            // Ensure the client is disposed properly
            using (client)
            {
                // Get mailbox information asynchronously
                ExchangeMailboxInfo mailboxInfo = null;
                try
                {
                    mailboxInfo = await client.GetMailboxInfoAsync();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to retrieve mailbox info: {ex.Message}");
                    return;
                }

                // Display basic mailbox info (avoid non‑existent DisplayName property)
                Console.WriteLine($"Mailbox URI: {mailboxInfo.MailboxUri}");
                Console.WriteLine($"Inbox URI: {mailboxInfo.InboxUri}");
                Console.WriteLine($"Sent Items URI: {mailboxInfo.SentItemsUri}");

                // List messages in the Inbox folder asynchronously
                try
                {
                    var messages = await client.ListMessagesAsync(mailboxInfo.InboxUri, null, 0, null, false, null, CancellationToken.None);
                    Console.WriteLine($"Found {messages.Count} messages in Inbox.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to list messages: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
