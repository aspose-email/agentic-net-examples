using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;
using Aspose.Email.Mapi;

namespace Sample
{
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
                string tenantId = "tenantId";
                string folderId = "Inbox";

                // Verify the MSG file exists
                if (!File.Exists(msgPath))
                {
                    Console.Error.WriteLine($"Message file not found: {msgPath}");
                    return;
                }

                // Create token provider for Microsoft Graph
                Aspose.Email.Clients.ITokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);

                // Initialize Graph client
                using (IGraphClient client = GraphClient.GetClient(tokenProvider, tenantId))
                {
                    // Load the MSG file as a MapiMessage
                    using (MapiMessage mapiMessage = MapiMessage.Load(msgPath))
                    {
                        try
                        {
                            // Upload the message to the specified folder (acts as project creation)
                            client.CreateMessage(folderId, mapiMessage);
                            Console.WriteLine("Message uploaded successfully.");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to create message: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
