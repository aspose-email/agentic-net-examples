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
            // EWS service URL and credentials (replace with real values)
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create and use the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
            {
                // Folder identifier (e.g., Inbox)
                string folderId = client.MailboxInfo.InboxUri;

                // Find conversations in the specified folder
                ExchangeConversation[] conversations = client.FindConversations(folderId);
                if (conversations == null || conversations.Length == 0)
                {
                    Console.WriteLine("No conversations found.");
                    return;
                }

                foreach (ExchangeConversation conversation in conversations)
                {
                    Console.WriteLine($"Conversation Topic: {conversation.ConversationTopic}");
                    Console.WriteLine($"Conversation Id: {conversation.ConversationId}");

                    // Retrieve all messages belonging to the conversation
                    MailMessageCollection messages = client.FetchConversationMessages(conversation.ConversationId);
                    if (messages != null)
                    {
                        foreach (MailMessage msg in messages)
                        {
                            using (msg)
                            {
                                Console.WriteLine($"  Subject: {msg.Subject}");
                                Console.WriteLine($"  From: {msg.From?.DisplayName ?? msg.From?.Address}");
                                Console.WriteLine($"  Received: {msg.Date}");
                                Console.WriteLine(new string('-', 40));
                            }
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
