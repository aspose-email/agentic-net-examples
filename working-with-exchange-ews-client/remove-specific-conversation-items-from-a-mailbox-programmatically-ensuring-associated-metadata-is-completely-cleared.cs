using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        // Author note: This sample demonstrates how to delete specific conversation items from an Exchange mailbox using EWS.
        // Connection parameters – replace with actual values.
        string serviceUrl = "https://mail.example.com/EWS/Exchange.asmx";
        string username = "user@example.com";
        string password = "password";


        // Skip external calls when placeholder credentials are used
        if (serviceUrl.Contains("example.com") || username.Contains("example.com") || password == "password")
        {
            Console.Error.WriteLine("Placeholder credentials detected. Skipping external calls.");
            return;
        }

        try
        {
            // Create and dispose the EWS client.
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, username, password))
            {
                // Retrieve mailbox information to obtain the Inbox folder URI.
                ExchangeMailboxInfo mailboxInfo = client.GetMailboxInfo();
                string inboxUri = mailboxInfo.InboxUri;

                // Find all conversations in the Inbox folder.
                ExchangeConversation[] conversations = client.FindConversations(inboxUri);

                foreach (ExchangeConversation conversation in conversations)
                {
                    // Fetch messages belonging to the current conversation.
                    MailMessageCollection messages = client.FetchConversationMessages(conversation.ConversationId);

                    // Ensure there is at least one message to examine.
                    if (messages != null && messages.Count > 0)
                    {
                        // Use the subject of the first message as a simple filter criterion.
                        string subject = messages[0].Subject;
                        if (!string.IsNullOrEmpty(subject) && subject.Contains("Test"))
                        {
                            // Delete all items of the matching conversation.
                            client.DeleteConversationItems(conversation.ConversationId);
                            Console.WriteLine($"Deleted conversation with subject: {subject}");
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
