using Aspose.Email;
using Aspose.Email.Clients;
using System;

namespace GmailAuthCodeSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // OAuth 2.0 token endpoint for Google
                string requestUrl = "https://oauth2.googleapis.com/token";

                // Replace the following placeholders with your actual credentials
                string clientId = "YOUR_CLIENT_ID";
                string clientSecret = "YOUR_CLIENT_SECRET";
                string refreshToken = "YOUR_REFRESH_TOKEN";

                // Guard against placeholder values
                if (clientId.StartsWith("YOUR_") ||
                    clientSecret.StartsWith("YOUR_") ||
                    refreshToken.StartsWith("YOUR_"))
                {
                    Console.Error.WriteLine("Error: Please replace the placeholder values with actual credentials before running the sample.");
                    return;
                }

                // Create a TokenProvider instance using the required GetInstance overload
                TokenProvider tokenProvider = TokenProvider.GetInstance(requestUrl, clientId, clientSecret, refreshToken);
                using (tokenProvider)
                {
                    // Retrieve the OAuth access token
                    OAuthToken oauthToken = tokenProvider.GetAccessToken();

                    // Output the access token (authorization code equivalent for this flow)
                    Console.WriteLine("Access Token: " + oauthToken.Token);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
