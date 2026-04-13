using Aspose.Email;
using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Mapi;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Mailbox URI and credentials
            string mailboxUri = "https://outlook.office365.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Cancellation token that aborts after 10 seconds
            using CancellationTokenSource cts = new CancellationTokenSource(TimeSpan.FromSeconds(10));

            // Initialize asynchronous EWS client
            using (IAsyncEwsClient client = await EWSClient.GetEwsClientAsync(mailboxUri, credentials, cancellationToken: cts.Token))
            {
                // URI of the item to fetch (replace with a real item URI)
                string itemUri = "https://outlook.office365.com/EWS/Exchange.asmx/YourItemUri";

                // Attempt to fetch the item with the cancellation token
                MapiMessage message = await client.FetchItemAsync(itemUri, null, cts.Token);
                Console.WriteLine("Fetched item subject: " + message.Subject);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
