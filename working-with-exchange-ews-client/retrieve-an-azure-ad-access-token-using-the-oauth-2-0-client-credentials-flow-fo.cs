using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

class Program
{
    static void Main()
    {
        try
        {
            // Replace the placeholders with real values
            string requestUrl = "https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token";
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = ""; // Not used for client‑credentials flow
            string tenantId = "your-tenant-id";

            // Create a token provider for the client‑credentials flow
            using (TokenProvider tokenProvider = TokenProvider.GetInstance(requestUrl, clientId, clientSecret, refreshToken))
            {
                // Acquire an OAuth token
                var token = tokenProvider.GetAccessToken();

                // Initialize the Graph client with the token provider
                using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, tenantId))
                {
                    // The access token is now ready for authenticated API calls
                    Console.WriteLine("Access token acquired successfully.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}