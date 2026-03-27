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
            // Paths and authentication parameters (replace with real values)
            string msgPath = "sample.msg";
            string clientId = "clientId";
            string clientSecret = "clientSecret";
            string refreshToken = "refreshToken";
            string tenantId = "tenantId";

            // Verify that the MSG file exists before attempting to load it
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine($"Message file not found: {msgPath}");
                return;
            }

            // Load the MSG file into a MailMessage instance
            using (MailMessage mailMessage = MailMessage.Load(msgPath))
            {
                // Create a token provider for Outlook (Microsoft Graph)
                TokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);

                // Initialize the Graph client using the token provider and tenant identifier
                using (IGraphClient client = GraphClient.GetClient(tokenProvider, tenantId))
                {
                    // Destination folder identifier (e.g., "Inbox")
                    string folderId = "Inbox";

                    // Upload the message to the specified folder
                    client.CreateMessage(folderId, mailMessage);
                    Console.WriteLine("Message uploaded successfully.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
