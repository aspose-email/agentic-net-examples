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
                // ----- Input parameters (replace placeholders with real values) -----
                string msgPath = "YOUR_MSG_PATH.msg";
                string requestUrl = "YOUR_REQUEST_URL"; // e.g., "https://login.microsoftonline.com/common/oauth2/v2.0/token"
                string clientId = "YOUR_CLIENT_ID";
                string clientSecret = "YOUR_CLIENT_SECRET";
                string refreshToken = "YOUR_REFRESH_TOKEN";
                string tenantId = "YOUR_TENANT_ID";

                // Guard placeholders
                if (msgPath.Contains("YOUR_") ||
                    requestUrl.Contains("YOUR_") ||
                    clientId.Contains("YOUR_") ||
                    clientSecret.Contains("YOUR_") ||
                    refreshToken.Contains("YOUR_") ||
                    tenantId.Contains("YOUR_"))
                {
                    Console.Error.WriteLine("One or more required parameters contain placeholder values. Please replace them with actual values.");
                    return;
                }

                // Guard file existence
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

                    Console.Error.WriteLine($"MSG file not found at path: {msgPath}");
                    return;
                }

                // Load the MSG file
                MapiMessage mapMsg = MapiMessage.Load(msgPath);
                string subject = mapMsg.Subject ?? string.Empty;
                Console.WriteLine($"Loaded MSG with subject: {subject}");

                // Create token provider for Microsoft Graph
                using (TokenProvider tokenProvider = TokenProvider.GetInstance(requestUrl, clientId, clientSecret, refreshToken))
                {
                    // Initialize Graph client
                    using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, tenantId))
                    {
                        // Example: Here you would use graphClient to configure permissions based on data extracted from the MSG.
                        // For instance, you could grant a user read access to a DriveItem.
                        // The actual API calls depend on your scenario and are omitted for brevity.

                        Console.WriteLine("Graph client initialized successfully.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
