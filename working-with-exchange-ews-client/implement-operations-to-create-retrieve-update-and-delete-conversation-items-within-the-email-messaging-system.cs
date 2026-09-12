using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Mapi;

namespace AsposeEmailConversationSample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // EWS service connection parameters (replace with real values)
                string serviceUrl = "https://outlook.office365.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Skip external calls when placeholder credentials are used
                if (username.Contains("example.com") || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create the async EWS client (explicit cast from sync client)
                IAsyncEwsClient ewsClient = (IAsyncEwsClient)EWSClient.GetEWSClient(serviceUrl, username, password);

                // Retrieve mailbox information to obtain folder URIs
                var mailboxInfo = await ewsClient.GetMailboxInfoAsync(null, CancellationToken.None);
                string inboxUri = mailboxInfo.InboxUri;
                string deletedItemsUri = mailboxInfo.DeletedItemsUri;

                // -----------------------------------------------------------------
                // 1. Create a new mail message and store it as a conversation item
                // -----------------------------------------------------------------
                var mail = new MailMessage
                {
                    From = new MailAddress("sender@example.com"),
                    Subject = "Conversation Sample",
                    Body = "This is a sample message for conversation CRUD operations."
                };
                mail.To.Add(new MailAddress("recipient@example.com"));

                // Convert MailMessage to MapiMessage
                MapiMessage mapiMessage = MapiMessage.FromMailMessage(mail);

                // Create the item in the Inbox folder
                string createdItemId = await ewsClient.CreateItemAsync(mapiMessage, inboxUri, CancellationToken.None);
                Console.WriteLine($"Created item ID: {createdItemId}");

                // For demonstration, use the created item ID as the conversation ID
                string conversationId = createdItemId;

                // ---------------------------------------------------------------
                // 2. Retrieve all messages belonging to the conversation
                // ---------------------------------------------------------------
                MailMessageCollection conversationMessages = await ewsClient.FetchConversationMessagesAsync(conversationId, CancellationToken.None);
                Console.WriteLine($"Conversation contains {conversationMessages.Count} message(s).");
                foreach (MailMessage msg in conversationMessages)
                {
                    Console.WriteLine($"- Subject: {msg.Subject}");
                }

                // ---------------------------------------------------------------
                // 3. Move the conversation items to Deleted Items folder
                // ---------------------------------------------------------------
                await ewsClient.MoveConversationItemsAsync(conversationId, deletedItemsUri, inboxUri, CancellationToken.None);
                Console.WriteLine("Conversation items moved to Deleted Items.");

                // ---------------------------------------------------------------
                // 4. Delete the conversation items permanently
                // ---------------------------------------------------------------
                await ewsClient.DeleteConversationItemsAsync(conversationId, deletedItemsUri, CancellationToken.None);
                Console.WriteLine("Conversation items deleted.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
