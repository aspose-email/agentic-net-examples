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
            // Placeholder connection details
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Skip real network calls when using placeholder credentials
            if (mailboxUri.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    // Get Inbox folder URI
                    string inboxUri = client.MailboxInfo.InboxUri;

                    // Find conversations in the Inbox
                    ExchangeConversation[] conversations = client.FindConversations(inboxUri);
                    if (conversations == null || conversations.Length == 0)
                    {
                        Console.WriteLine("No conversations found.");
                        return;
                    }

                    foreach (ExchangeConversation conversation in conversations)
                    {
                        Console.WriteLine($"Conversation ID: {conversation.ConversationId}");
                        Console.WriteLine($"Topic: {conversation.ConversationTopic}");
                        Console.WriteLine($"Total messages in conversation: {conversation.MessageCount}");

                        // Fetch all messages belonging to this conversation
                        MailMessageCollection messages = client.FetchConversationMessages(conversation.ConversationId);
                        foreach (MailMessage message in messages)
                        {
                            using (message)
                            {
                                Console.WriteLine("--------------------------------------------------");
                                Console.WriteLine($"Subject: {message.Subject}");
                                Console.WriteLine($"From: {message.From}");
                                Console.WriteLine($"Date: {message.Date}");
                            }
                        }
                        Console.WriteLine();
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during EWS operations: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
