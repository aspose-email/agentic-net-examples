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
            // Initialize the EWS client with server URL and credentials.
            using (IEWSClient client = EWSClient.GetEWSClient(
                "https://exchange.example.com/EWS/Exchange.asmx",
                new NetworkCredential("user@example.com", "password")))
            {
                // Specify the conversation ID to be removed.
                string conversationId = "YOUR_CONVERSATION_ID";

                // Delete all items belonging to the specified conversation.
                client.DeleteConversationItems(conversationId);

                Console.WriteLine("Conversation items deleted successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
