using System;
using Aspose.Email;
using Aspose.Email.Clients;

class Program
{
    static void Main()
    {
        try
        {
            // Azure AD token endpoint and client credentials (replace with real values)
            string requestUrl = "https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token";
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = ""; // not used for client‑credentials flow

            // Guard against placeholder credentials to avoid external calls during CI
            if (string.IsNullOrWhiteSpace(clientId) || clientId.StartsWith("your-"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping token request.");
                return;
            }

            using (TokenProvider provider = TokenProvider.GetInstance(requestUrl, clientId, clientSecret, refreshToken))
            {
                Aspose.Email.Clients.OAuthToken token = provider.GetAccessToken();
                string accessToken = token.Token;
                Console.WriteLine("Access Token: " + accessToken);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
