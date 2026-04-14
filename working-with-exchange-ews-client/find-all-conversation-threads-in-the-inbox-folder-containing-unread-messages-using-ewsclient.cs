using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

namespace FindUnreadConversations
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // EWS service URL and credentials
                string serviceUrl = "https://mail.example.com/EWS/Exchange.asmx";
                string username = "username";
                string password = "password";


                // Skip external calls when placeholder credentials are used
                if (serviceUrl.Contains("example.com") || username == "username" || password == "password")
                {
                    Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                    return;
                }

                // Create EWS client
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
                {
                    // Get the Inbox folder URI
                    string inboxFolderUri = client.MailboxInfo.InboxUri;

                    // Find all conversations in the Inbox folder
                    ExchangeConversation[] conversations = client.FindConversations(inboxFolderUri);

                    // Iterate and display conversations that contain unread messages
                    foreach (ExchangeConversation conversation in conversations)
                    {
                        if (conversation.UnreadCount > 0)
                        {
                            Console.WriteLine($"Conversation ID: {conversation.ConversationId}");
                            Console.WriteLine($"Topic: {conversation.ConversationTopic}");
                            Console.WriteLine($"Unread Count: {conversation.UnreadCount}");
                            Console.WriteLine();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
