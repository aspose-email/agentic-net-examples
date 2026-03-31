using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder connection details
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Get Inbox folder identifier
                string inboxFolderId = client.MailboxInfo.InboxUri;

                // Find conversations in the Inbox
                ExchangeConversation[] conversations = client.FindConversations(inboxFolderId);
                if (conversations == null || conversations.Length == 0)
                {
                    Console.WriteLine("No conversations found in the Inbox.");
                    return;
                }

                // Use the first conversation for demonstration
                string conversationId = conversations[0].ConversationId;
                Console.WriteLine($"Conversation ID: {conversationId}");

                // Retrieve messages belonging to the conversation
                MailMessageCollection messages = client.FetchConversationMessages(conversationId);
                Console.WriteLine($"Number of messages in conversation: {messages.Count}");

                // Mark the conversation as read
                client.SetConversationReadState(conversationId, true);
                Console.WriteLine("Conversation marked as read.");

                // Copy the conversation to the Drafts folder
                string draftsFolderId = client.MailboxInfo.DraftsUri;
                client.CopyConversationItems(conversationId, draftsFolderId);
                Console.WriteLine("Conversation copied to Drafts folder.");

                // Move the conversation to the Deleted Items folder
                string deletedItemsFolderId = client.MailboxInfo.DeletedItemsUri;
                client.MoveConversationItems(conversationId, deletedItemsFolderId);
                Console.WriteLine("Conversation moved to Deleted Items folder.");

                // Delete the conversation items from Deleted Items (cleanup)
                client.DeleteConversationItems(conversationId, deletedItemsFolderId);
                Console.WriteLine("Conversation items deleted from Deleted Items folder.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
