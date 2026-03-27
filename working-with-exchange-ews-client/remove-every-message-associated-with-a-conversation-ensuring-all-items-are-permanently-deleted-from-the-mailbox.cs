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
            // Initialize the EWS client
            string serviceUrl = "https://exchange.example.com/EWS/Exchange.asmx";
            NetworkCredential credentials = new NetworkCredential("username", "password");
            using (IEWSClient client = EWSClient.GetEWSClient(serviceUrl, credentials))
            {
                // ID of the conversation to delete
                string conversationId = "conversation-id";

                // Delete all items belonging to the conversation permanently
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
