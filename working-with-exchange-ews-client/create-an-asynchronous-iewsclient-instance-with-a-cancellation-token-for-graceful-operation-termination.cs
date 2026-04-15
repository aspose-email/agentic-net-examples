using Aspose.Email.Clients.Exchange;
using Aspose.Email;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Create a cancellation token source for graceful termination.
            using CancellationTokenSource cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(30));
            CancellationToken token = cts.Token;

            // Prepare credentials.
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Mailbox URI (replace with actual endpoint).
            string mailboxUri = "https://outlook.office365.com/EWS/Exchange.asmx";

            // Create the asynchronous EWS client.
            IAsyncEwsClient client = null;
            try
            {
                client = await EWSClient.GetEwsClientAsync(mailboxUri, credentials, null, token);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            // Example operation: retrieve mailbox information.
            try
            {
                ExchangeMailboxInfo mailboxInfo = await client.GetMailboxInfoAsync(null, token);
                Console.WriteLine($"Mailbox display name: {mailboxInfo.MailboxUri}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Operation failed: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
