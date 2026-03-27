using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

        try
        {
            // Placeholder values – replace with actual server URI and credentials.
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");

            // Create and dispose the EWS client.
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, credentials))
            {
                // Create a simple email message.
                using (MailMessage message = new MailMessage("sender@example.com", "recipient@example.com", "Conversation Sample", "This is a test message for conversation."))
                {
                    // Send the message to the mailbox.
                    client.Send(message);
                }

                // Identify the Inbox folder URI.
                string inboxFolderId = client.MailboxInfo.InboxUri;

                // Find conversations in the Inbox.
                ExchangeConversation[] conversations = client.FindConversations(inboxFolderId);
                if (conversations.Length == 0)
                {
                    Console.WriteLine("No conversations found.");
                    return;
                }

                // Use the first conversation for demonstration.
                ExchangeConversation conversation = conversations[0];
                Console.WriteLine("Conversation ID: " + conversation.ConversationId);
                Console.WriteLine("Conversation Topic: " + conversation.ConversationTopic);

                // Retrieve all messages belonging to the conversation.
                MailMessageCollection messages = client.FetchConversationMessages(conversation.ConversationId);
                foreach (MailMessage msg in messages)
                {
                    Console.WriteLine("Message Subject: " + msg.Subject);
                    msg.Dispose();
                }

                // Update: mark the entire conversation as read.
                client.SetConversationReadState(conversation.ConversationId, inboxFolderId, true);
                Console.WriteLine("Conversation marked as read.");

                // Delete all items of the conversation from the Inbox.
                client.DeleteConversationItems(conversation.ConversationId, inboxFolderId);
                Console.WriteLine("Conversation items deleted.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}