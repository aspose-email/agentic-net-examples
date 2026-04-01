using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        // Top-level exception guard
        try
        {
            // Placeholder credentials – skip execution in CI environments
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            if (mailboxUri.Contains("example.com") ||
                string.Equals(username, "username", StringComparison.OrdinalIgnoreCase) ||
                string.Equals(password, "password", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping execution.");
                return;
            }

            // Create the EWS client using the verified factory method
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                // Conversation identifier to be removed
                string conversationId = "YOUR_CONVERSATION_ID";

                // Optional: specify a folder URI to limit deletion; null deletes from all folders
                string contextFolderId = null;

                try
                {
                    // Delete all items belonging to the specified conversation
                    client.DeleteConversationItems(conversationId, contextFolderId);
                    Console.WriteLine("Conversation items deleted successfully.");
                }
                catch (Exception ex)
                {
                    // Client‑level error handling
                    Console.Error.WriteLine($"Error deleting conversation items: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            // Global error handling
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
