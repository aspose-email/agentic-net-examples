using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Clients.Exchange;

class Program
{
    static void Main()
    {
        try
        {
            // EWS service connection parameters
            string serviceUrl = "https://outlook.office365.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";


            // Skip external calls when placeholder credentials are used
            if (username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
                return;
            }

            // Create the EWS client
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Get the Inbox folder identifier
                string inboxFolderId = client.GetMailboxInfo().InboxUri;

                // Find all conversations in the Inbox
                ExchangeConversation[] conversations = client.FindConversations(inboxFolderId);

                foreach (ExchangeConversation conversation in conversations)
                {
                    // Retrieve all messages belonging to the current conversation
                    MailMessageCollection messages = client.FetchConversationMessages(conversation.GetHashCode().ToString());

                    Console.WriteLine($"Conversation ID: {conversation.GetHashCode()} - Message count: {messages.Count}");

                    foreach (MailMessage message in messages)
                    {
                        Console.WriteLine($"Subject: {message.Subject}");
                    }

                    Console.WriteLine(new string('-', 40));
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
