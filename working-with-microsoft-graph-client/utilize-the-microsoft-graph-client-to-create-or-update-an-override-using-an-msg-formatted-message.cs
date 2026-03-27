using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

namespace AsposeEmailGraphExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Initialize token provider (replace placeholders with real values)
                Aspose.Email.Clients.ITokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                    "clientId",
                    "clientSecret",
                    "refreshToken");

                string tenantId = "tenantId";

                // Create Graph client
                using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, tenantId))
                {
                    // Path to the MSG file
                    string msgPath = "message.msg";

                    // Verify the MSG file exists
                    if (!File.Exists(msgPath))
                    {
                        Console.Error.WriteLine($"File not found: {msgPath}");
                        return;
                    }

                    // Load the MSG file into a MapiMessage
                    using (MapiMessage mapiMessage = MapiMessage.Load(msgPath))
                    {
                        // Target folder identifier (e.g., "Inbox")
                        string folderId = "Inbox";

                        // Create (or update) the message in the specified folder
                        MapiMessage createdMessage = graphClient.CreateMessage(folderId, mapiMessage);

                        Console.WriteLine($"Message created. ItemId: {createdMessage.ItemId}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}