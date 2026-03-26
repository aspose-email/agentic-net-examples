using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Graph;

class Program
{
    static void Main()
    {
        try
        {
            // Obtain OAuth 2.0 tokens (replace with real implementation)
            TokenResponse tokenResponse = GetOAuthToken();

            // Create a token provider (Outlook example)
            TokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(
                clientId: "your-client-id",
                clientSecret: "your-client-secret",
                refreshToken: tokenResponse.RefreshToken);

            // Initialize Graph client with the token provider
            using (IGraphClient graphClient = GraphClient.GetClient(tokenProvider, tenantId: "your-tenant-id"))
            {
                // Example API request (commented out – replace with actual usage)
                // var messages = graphClient.ListMessages("Inbox");
                // foreach (var msg in messages)
                // {
                //     Console.WriteLine(msg.Subject);
                // }

                Console.WriteLine("Graph client configured successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Mock method to simulate obtaining an OAuth token response
    static TokenResponse GetOAuthToken()
    {
        // Replace this stub with actual OAuth 2.0 flow to retrieve tokens
        return new TokenResponse
        {
            AccessToken = "access-token-placeholder",
            RefreshToken = "refresh-token-placeholder"
        };
    }
}

// Simple representation of an OAuth token response
class TokenResponse
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}