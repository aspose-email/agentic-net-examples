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
            // Initialize credentials and service URL (replace with actual values)
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create the synchronous EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
            {
                // Cast to async client interface
                IAsyncEwsClient asyncClient = client as IAsyncEwsClient;
                if (asyncClient == null)
                {
                    Console.Error.WriteLine("Async EWS client is not available.");
                    return;
                }

                // Define the destination folder URI (e.g., Drafts folder)
                string destinationFolderUri = client.MailboxInfo.DraftsUri;

                // List messages in the Inbox folder
                ExchangeMessageInfoCollection messages = client.ListMessages(client.MailboxInfo.InboxUri);
                foreach (ExchangeMessageInfo msgInfo in messages)
                {
                    // Move each message to the destination folder
                    string movedItemUri = await asyncClient.MoveItemAsync(msgInfo.UniqueUri, destinationFolderUri);
                    Console.WriteLine($"Moved message {msgInfo.Subject} to destination. New URI: {movedItemUri}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
