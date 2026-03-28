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
            // Initialize the EWS client (replace placeholders with actual values)
            using (IEWSClient client = EWSClient.GetEWSClient(
                "https://exchange.example.com/EWS/Exchange.asmx",
                new NetworkCredential("username", "password")))
            {
                try
                {
                    // Find all conversations in the Inbox folder
                    string inboxFolderId = client.MailboxInfo.InboxUri;
                    ExchangeConversation[] conversations = client.FindConversations(inboxFolderId);

                    // Delete all items belonging to each conversation
                    foreach (ExchangeConversation conversation in conversations)
                    {
                        client.DeleteConversationItems(conversation.ConversationId);
                    }

                    Console.WriteLine("All conversation items have been permanently deleted.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing conversations: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Failed to initialize client or execute operation: {ex.Message}");
        }
    }
}
