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
            // Paths and placeholder credentials
            const string msgFilePath = "sample.msg";
            const string clientId = "YOUR_CLIENT_ID";
            const string clientSecret = "YOUR_CLIENT_SECRET";
            const string refreshToken = "YOUR_REFRESH_TOKEN";
            const string tenantId = "YOUR_TENANT_ID";
            const string folderId = "drafts"; // Target folder in Graph (e.g., Drafts)

            // Guard against placeholder credentials – skip execution if not provided
            if (clientId.StartsWith("YOUR_") || clientSecret.StartsWith("YOUR_") || refreshToken.StartsWith("YOUR_") || tenantId.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Graph operations.");
                return;
            }

            // Ensure the MSG file exists; create a minimal placeholder if missing
            if (!File.Exists(msgFilePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                try
                {
                    using (var placeholderMsg = new MapiMessage())
                    {
                        placeholderMsg.Save(msgFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file into a MapiMessage
            MapiMessage mapiMessage;
            try
            {
                mapiMessage = MapiMessage.Load(msgFilePath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            // Initialize the token provider
            Aspose.Email.Clients.ITokenProvider tokenProvider;
            try
            {
                tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create token provider: {ex.Message}");
                return;
            }

            // Create and use the Graph client
            using (IGraphClient client = GraphClient.GetClient(tokenProvider, tenantId))
            {
                try
                {
                    // Create the message in the specified folder
                    MapiMessage createdMessage = client.CreateMessage(folderId, mapiMessage);
                    Console.WriteLine("Message created successfully in folder '{0}'.", folderId);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Graph operation failed: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
