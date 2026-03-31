using System;
using Aspose.Email.Clients;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder OAuth parameters – replace with real values when available.
            string requestUrl = "https://login.microsoftonline.com/common/oauth2/v2.0/token";
            string clientId = "clientId";
            string clientSecret = "clientSecret";
            string authorizationCode = "authCode";

            // Guard against executing network calls with placeholder credentials.
            if (clientId == "clientId" || clientSecret == "clientSecret" || authorizationCode == "authCode")
            {
                Console.Error.WriteLine("Placeholder OAuth parameters detected. Skipping token exchange.");
                return;
            }

            // Create a TokenProvider instance for Outlook using the OAuth2 token endpoint.
            using (TokenProvider tokenProvider = TokenProvider.GetInstance(requestUrl, clientId, clientSecret, authorizationCode))
            {
                // Request an access token; the provider will handle the exchange.
                var oauthToken = tokenProvider.GetAccessToken();

                // The returned token may contain a refresh token; output it if available.
                Console.WriteLine("Access Token: " + oauthToken.Token);
                // Assuming the token object exposes a RefreshToken property.
                // Console.WriteLine("Refresh Token: " + oauthToken.RefreshToken);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
