using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

namespace AsposeEmailExample
{
    class Program
    {
        static void Main(string[] args)
        {
        var credential = new System.Net.NetworkCredential("username", "password", "domain");

            try
            {
                // Mailbox connection parameters (replace with real values)
                string mailboxUri = "https://exchange.example.com/EWS/Exchange.asmx";
                string username = "user@example.com";
                string password = "password";

                // Create the EWS client using the factory method
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    // Identifier of the conversation to be removed (replace with a real ID)
                    string conversationId = "AAMkAD...";

                    // Delete all items belonging to the specified conversation.
                    // This operation removes the messages and clears associated metadata.
                    client.DeleteConversationItems(conversationId);

                    Console.WriteLine("Conversation items deleted successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}