using Aspose.Email;
using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

class Program
{
    static void Main()
    {
        try
        {
            // Author note: replace placeholder values with real credentials.
            string requestUrl = "https://login.microsoftonline.com/common/oauth2/v2.0/token";
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";
            string tenantId = "your-tenant-id";

            // Create the token provider (implements IDisposable)
            using (TokenProvider tokenProvider = TokenProvider.GetInstance(requestUrl, clientId, clientSecret, refreshToken))
            {
                // Initialize the Graph client (also IDisposable)
                using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, tenantId))
                {
                    // Example operation: retrieve the default endpoint (no network call)
                    Console.WriteLine($"Graph endpoint: {graphClient.EndPoint}");
                    // Add further Graph API calls here, e.g., list messages, send mail, etc.
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
