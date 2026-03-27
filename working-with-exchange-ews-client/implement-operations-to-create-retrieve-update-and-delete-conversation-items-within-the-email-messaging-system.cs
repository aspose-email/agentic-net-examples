using System;
using System.Net;
using System.Linq;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Tools.Search;

class Program
{
    static void Main()
    {
        try
        {
            // Define connection parameters (replace with real values)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create and connect the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
            {
                // ------------------- Create a conversation (send a message) -------------------
                using (MailMessage message = new MailMessage(
                    "sender@example.com",
                    "recipient@example.com",
                    "Sample Conversation Subject",
                    "This is the body of the first message in the conversation."))
                {
                    client.Send(message);
                    Console.WriteLine("Message sent to start a conversation.");
                }

                // ------------------- Retrieve conversations from the Inbox -------------------
                string inboxFolderId = client.MailboxInfo.InboxUri;
                ExchangeConversation[] conversations = client.FindConversations(inboxFolderId);
                if (conversations == null || conversations.Length == 0)
                {
                    Console.WriteLine("No conversations found in the Inbox.");
                    return;
                }

                // Extract conversation identifiers
                string[] conversationIds = conversations
                    .Select(conv => conv.ConversationId)
                    .ToArray();

                Console.WriteLine($"Found {conversationIds.Length} conversation(s).");

                // ------------------- Fetch messages of the first conversation -------------------
                string firstConversationId = conversationIds[0];
                MailMessageCollection messages = client.FetchConversationMessages(firstConversationId);
                Console.WriteLine($"Messages in conversation '{firstConversationId}':");
                foreach (MailMessage msg in messages)
                {
                    Console.WriteLine($"- Subject: {msg.Subject}");
                }

                // ------------------- Update conversation read state (mark as read) -------------------
                client.SetConversationReadState(firstConversationId, true);
                Console.WriteLine("Conversation marked as read.");

                // ------------------- Delete all items of the conversation -------------------
                client.DeleteConversationItems(firstConversationId);
                Console.WriteLine("Conversation items deleted.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
