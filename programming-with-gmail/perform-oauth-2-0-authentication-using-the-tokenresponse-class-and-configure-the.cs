using System;
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
                // Dummy OAuth credentials – replace with real values.
                string clientId = "your-client-id";
                string clientSecret = "your-client-secret";
                string refreshToken = "your-refresh-token";
                string tenantId = "your-tenant-id";

                // Create a token provider for Google (used here as an example OAuth provider).
                TokenProvider tokenProvider = TokenProvider.Google.GetInstance(clientId, clientSecret, refreshToken);

                // Initialize the Graph client using the token provider.
                using (IGraphClient client = GraphClient.GetClient(tokenProvider, tenantId))
                {
                    // The client is now authenticated and ready for API calls.
                    // Example: list the user's mail folders (replace with actual calls as needed).
                    try
                    {
                        var folders = client.ListFolders();
                        foreach (var folder in folders)
                        {
                            Console.WriteLine($"Folder: {folder.DisplayName}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error while accessing Graph API: {ex.Message}");
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
