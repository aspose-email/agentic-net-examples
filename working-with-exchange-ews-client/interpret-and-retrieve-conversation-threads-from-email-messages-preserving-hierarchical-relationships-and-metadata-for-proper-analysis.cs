using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

namespace Example
{
    class Program
    {
        static void Main()
        {
            try
            {
                // EWS service URL and user credentials
                string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "username";
                string password = "password";

                // Create the EWS client
                using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, new NetworkCredential(username, password)))
                {
                    // Get the Inbox folder URI
                    string inboxFolderUri = client.MailboxInfo.InboxUri;

                    // Find conversations in the Inbox folder
                    ExchangeConversation[] conversations = client.FindConversations(inboxFolderUri);

                    Console.WriteLine($"Found {conversations.Length} conversation(s) in Inbox.");

                    foreach (ExchangeConversation conversation in conversations)
                    {
                        Console.WriteLine("--------------------------------------------------");
                        Console.WriteLine($"Conversation ID   : {conversation.ConversationId}");
                        Console.WriteLine($"Topic             : {conversation.ConversationTopic}");
                        Console.WriteLine($"Total messages    : {conversation.MessageCount}");
                        Console.WriteLine($"Unread messages   : {conversation.UnreadCount}");

                        // Fetch all messages belonging to this conversation
                        MailMessageCollection messages = client.FetchConversationMessages(conversation.ConversationId);

                        foreach (MailMessage message in messages)
                        {
                            using (MailMessage msg = message)
                            {
                                Console.WriteLine($"  Subject : {msg.Subject}");
                                Console.WriteLine($"  From    : {msg.From?.DisplayName ?? msg.From?.Address}");
                                Console.WriteLine($"  Date    : {msg.Date}");
                                Console.WriteLine();
                            }
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
