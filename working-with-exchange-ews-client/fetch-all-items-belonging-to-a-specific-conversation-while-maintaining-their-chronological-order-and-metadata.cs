using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder connection details
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            // Guard against executing real network calls with placeholder data
            if (serviceUrl.Contains("example.com") || username == "username" || password == "password")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping network call.");
                return;
            }

            // Create the EWS client
            IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password);
            using (client)
            {
                // Folder to search for conversations (Inbox)
                string folderId = client.MailboxInfo.InboxUri;

                // Find conversations in the folder
                ExchangeConversation[] conversations = client.FindConversations(folderId);
                if (conversations == null || conversations.Length == 0)
                {
                    Console.WriteLine("No conversations found.");
                    return;
                }

                // Specify the conversation ID to fetch (placeholder)
                string conversationId = "conversation-id-placeholder";

                // Verify the conversation exists
                ExchangeConversation targetConversation = null;
                foreach (ExchangeConversation conv in conversations)
                {
                    if (conv.ConversationId == conversationId)
                    {
                        targetConversation = conv;
                        break;
                    }
                }

                if (targetConversation == null)
                {
                    Console.WriteLine($"Conversation with ID '{conversationId}' not found.");
                    return;
                }

                // Fetch all messages belonging to the conversation
                MailMessageCollection messages = client.FetchConversationMessages(conversationId);
                if (messages == null || messages.Count == 0)
                {
                    Console.WriteLine("No messages found for the specified conversation.");
                    return;
                }

                // Sort messages chronologically (oldest first)
                List<MailMessage> sortedMessages = new List<MailMessage>(messages);
                sortedMessages.Sort((x, y) => DateTime.Compare(x.Date, y.Date));

                // Output metadata for each message
                foreach (MailMessage message in sortedMessages)
                {
                    Console.WriteLine("Subject: " + message.Subject);
                    Console.WriteLine("From: " + (message.From != null ? message.From.Address : "N/A"));
                    Console.WriteLine("Date: " + message.Date.ToString("u"));
                    Console.WriteLine("--------------------------------------------------");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
