using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the source MSG file
            string msgPath = "sample.msg";

            // Verify that the MSG file exists
            if (!File.Exists(msgPath))
            {
                Console.Error.WriteLine("Message file not found: " + msgPath);
                return;
            }

            // Initialize the token provider (replace placeholders with real values)
            Aspose.Email.Clients.ITokenProvider tokenProvider;
            try
            {
                tokenProvider = TokenProvider.Outlook.GetInstance("clientId", "clientSecret", "refreshToken");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Failed to create token provider: " + ex.Message);
                return;
            }

            // Create the Graph client
            using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, "tenantId"))
            {
                // Load the MSG file as a MapiMessage
                using (MapiMessage mapiMessage = MapiMessage.FromMailMessage(msgPath))
                {
                    // Destination folder identifier (e.g., "Inbox")
                    string destinationFolderId = "Inbox";

                    // Upload the message to the destination folder
                    MapiMessage createdMessage = graphClient.CreateMessage(destinationFolderId, mapiMessage);
                    Console.WriteLine("Message copied successfully. Subject: " + createdMessage?.Subject);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}