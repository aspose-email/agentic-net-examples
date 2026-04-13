using System;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange.WebService.Models;
using Aspose.Email.Mapi;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Placeholder credentials guard
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) ||
                username.Equals("username", StringComparison.OrdinalIgnoreCase) ||
                password.Equals("password", StringComparison.OrdinalIgnoreCase))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network operation.");
                return;
            }

            // Create EWS client
            IEWSClient client = null;
            try
            {
                client = EWSClient.GetEWSClient(mailboxUri, username, password);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            // Prepare a draft MAPI message
            MapiMessage mapiMessage = new MapiMessage("sender@example.com", "recipient@example.com", "Draft Subject", "This is a draft body.");
            // Add a custom flag as a Unicode property
            mapiMessage.AddCustomProperty(
                MapiPropertyType.PT_UNICODE,
                Encoding.Unicode.GetBytes("Flagged"),
                "X-Custom-Flag");

            // Determine Sent Items folder URI
            string sentFolderUri = client.MailboxInfo.SentItemsUri;
            if (string.IsNullOrEmpty(sentFolderUri))
            {
                Console.Error.WriteLine("Sent Items folder URI not available.");
                return;
            }

            // Build AppendMessage parameters
            EwsAppendMessage appendParams = EwsAppendMessage.Create()
                .SetFolder(sentFolderUri)
                .AddMessage(mapiMessage);

            // Cast to async client if supported
            if (client is IAsyncEwsClient asyncClient)
            {
                try
                {
                    var result = await asyncClient.AppendMessagesAsync(appendParams).ConfigureAwait(false);
                    foreach (var uri in result)
                    {
                        Console.WriteLine($"Draft appended to Sent Items: {uri}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to append draft asynchronously: {ex.Message}");
                }
            }
            else
            {
                // Fallback to synchronous AppendMessage with markAsSent flag
                try
                {
                    string draftUri = client.AppendMessage(sentFolderUri, mapiMessage, true);
                    Console.WriteLine($"Draft appended to Sent Items (sync): {draftUri}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to append draft synchronously: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
