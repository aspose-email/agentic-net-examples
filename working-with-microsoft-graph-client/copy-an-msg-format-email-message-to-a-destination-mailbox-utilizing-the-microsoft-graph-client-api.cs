using Aspose.Email.Mapi;
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
            // Placeholder credentials – replace with real values.
            string clientId = "your_client_id";
            string clientSecret = "your_client_secret";
            string refreshToken = "your_refresh_token";
            string tenantId = "your_tenant_id";
            string userId = "user@example.com";

            // Detect placeholder credentials and skip execution to avoid live calls.
            if (clientId.StartsWith("your_") ||
                clientSecret.StartsWith("your_") ||
                refreshToken.StartsWith("your_") ||
                tenantId.StartsWith("your_") ||
                userId.StartsWith("your_"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Graph operations.");
                return;
            }

            // Path to the local MSG file.
            string msgPath = "sample.msg";

            // Guard file existence.
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Message file not found: {msgPath}");
                return;
            }

            // Load the MSG file into a MapiMessage.
            MapiMessage sourceMessage;
            try
            {
                sourceMessage = MapiMessage.Load(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            // Folder identifiers – replace with actual folder IDs.
            string sourceFolderId = "source-folder-id";
            string destinationFolderId = "destination-folder-id";

            // Create a token provider.
            Aspose.Email.Clients.ITokenProvider tokenProvider = TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);

            // Initialize the Graph client.
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, "https://graph.microsoft.com"))
            {
                // Set tenant and resource (user) identifiers.
                client.TenantId = tenantId;
                client.ResourceId = userId;

                // Upload the message to the source folder.
                try
                {
                    client.CreateMessage(sourceFolderId, sourceMessage);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to upload message: {ex.Message}");
                    return;
                }

                // At this point, the message exists in the source folder.
                // The item ID of the uploaded message should be retrieved.
                // For demonstration, assume we have the item ID.
                string uploadedMessageId = "uploaded-message-id";

                // Copy the message to the destination folder.
                try
                {
                    MapiMessage copiedMessage = client.CopyMessage(destinationFolderId, uploadedMessageId);
                    Console.WriteLine($"Message copied successfully. New ID: {copiedMessage?.Subject}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to copy message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
