using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the EWS client (replace placeholders with real values)
            using (IEWSClient client = EWSClient.GetEWSClient("https://exchange.example.com/EWS/Exchange.asmx", "username", "password"))
            {
                try
                {
                    // Folder URI where the conversation resides (e.g., Inbox)
                    string folderUri = client.MailboxInfo.InboxUri;

                    // Identify the conversation to delete.
                    // In a real scenario, you would obtain this ID from FindConversations or other logic.
                    string conversationId = "YOUR_CONVERSATION_ID";

                    // Delete all items belonging to the specified conversation within the folder.
                    client.DeleteConversationItems(conversationId, folderUri);

                    Console.WriteLine("Conversation items deleted successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during conversation deletion: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
