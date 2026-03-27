using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

class Program
{
    static void Main()
    {
        try
        {
            // Paths and credentials (replace with real values)
            string msgPath = "sample.msg";
            string clientId = "clientId";
            string clientSecret = "clientSecret";
            string refreshToken = "refreshToken";
            string userEmail = "user@example.com";

            // Verify the MSG file exists
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            // Load the MSG file into a MailMessage
            using (MailMessage mailMessage = MailMessage.Load(msgPath))
            {
                // Create a token provider for Outlook (Microsoft Graph)
                Aspose.Email.Clients.TokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                    clientId, clientSecret, refreshToken);

                // Initialize the Graph client
                using (IGraphClient client = GraphClient.GetClient(tokenProvider, userEmail))
                {
                    // Destination folder identifier (e.g., "Inbox")
                    string destinationFolderId = "Inbox";

                    // Create (copy) the message into the destination mailbox folder
                    client.CreateMessage(destinationFolderId, mailMessage);
                    Console.WriteLine("Message copied successfully.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
