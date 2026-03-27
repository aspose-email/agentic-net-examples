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
            // Define connection parameters (replace with real values)
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create the EWS client using the factory method.
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, new NetworkCredential(username, password)))
            {
                // Get the Inbox folder identifier.
                string inboxFolderId = client.MailboxInfo.InboxUri;

                // Find all conversations in the Inbox.
                ExchangeConversation[] conversations = client.FindConversations(inboxFolderId);

                // Delete each conversation permanently.
                foreach (ExchangeConversation conversation in conversations)
                {
                    try
                    {
                        client.DeleteConversationItems(conversation.ConversationId);
                        Console.WriteLine($"Deleted conversation: {conversation.ConversationId}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to delete conversation {conversation.ConversationId}: {ex.Message}");
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