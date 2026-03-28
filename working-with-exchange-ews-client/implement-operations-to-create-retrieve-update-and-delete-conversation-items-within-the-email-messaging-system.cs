using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize EWS client
            IEWSClient client = null;
            try
            {
                client = EWSClient.GetEWSClient(
                    "https://example.com/EWS/Exchange.asmx",
                    new NetworkCredential("username", "password"));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create EWS client: {ex.Message}");
                return;
            }

            using (client)
            {
                // Find conversations in the Inbox folder
                ExchangeConversation[] conversations = null;
                try
                {
                    conversations = client.FindConversations(client.MailboxInfo.InboxUri);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error finding conversations: {ex.Message}");
                    return;
                }

                if (conversations == null || conversations.Length == 0)
                {
                    Console.WriteLine("No conversations found.");
                    return;
                }

                foreach (ExchangeConversation conv in conversations)
                {
                    Console.WriteLine($"Conversation ID: {conv.ConversationId}");
                    Console.WriteLine($"Topic: {conv.ConversationTopic}");
                    Console.WriteLine($"Message Count: {conv.MessageCount}");

                    // Retrieve messages of the conversation
                    MailMessageCollection messages = null;
                    try
                    {
                        messages = client.FetchConversationMessages(conv.ConversationId);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to fetch messages for conversation {conv.ConversationId}: {ex.Message}");
                        continue;
                    }

                    Console.WriteLine($"Fetched {messages.Count} messages.");

                    // Example update: mark all items in the conversation as read
                    try
                    {
                        client.SetConversationReadState(conv.ConversationId, true);
                        Console.WriteLine("Marked conversation as read.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to set read state for conversation {conv.ConversationId}: {ex.Message}");
                    }

                    // Example delete: delete all items of the conversation
                    // Uncomment the following block to perform deletion
                    /*
                    try
                    {
                        client.DeleteConversationItems(conv.ConversationId);
                        Console.WriteLine("Deleted conversation items.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to delete conversation {conv.ConversationId}: {ex.Message}");
                    }
                    */
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
