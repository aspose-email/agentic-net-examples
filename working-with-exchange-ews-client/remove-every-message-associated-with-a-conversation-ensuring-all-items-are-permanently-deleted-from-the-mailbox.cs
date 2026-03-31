using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        // Top‑level exception guard
        try
        {
            // Placeholder connection details
            string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
            string username = "user@example.com";
            string password = "password";

            // Create the EWS client using the correct factory method
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // Placeholder conversation identifier
                    string conversationId = "conversation-id";

                    // Attempt to delete all items belonging to the conversation permanently
                    try
                    {
                        client.DeleteConversationItems(conversationId);
                        Console.WriteLine("All messages in the conversation have been permanently deleted.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to delete conversation items: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to connect to Exchange server: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
