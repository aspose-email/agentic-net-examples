using Aspose.Email;
using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";

            // Guard against placeholder credentials to avoid external calls during CI
            if (clientId.StartsWith("your-") || clientSecret.StartsWith("your-") || refreshToken.StartsWith("your-"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping token request.");
                return;
            }

            // Obtain a token provider for Google and retrieve the OAuth token
            using (TokenProvider provider = TokenProvider.Google.GetInstance(clientId, clientSecret, refreshToken))
            {
                OAuthToken token = provider.GetAccessToken();
                Console.WriteLine($"Access Token: {token.Token}");
                // token.ExpiresIn, token.RefreshToken, etc., can be accessed if needed
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
