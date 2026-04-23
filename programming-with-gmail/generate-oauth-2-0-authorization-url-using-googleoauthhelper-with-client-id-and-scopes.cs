using Aspose.Email;
using System;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

namespace OAuthSample
{
    // Helper class to build Google OAuth 2.0 authorization URL
    public static class GoogleOAuthHelper
    {
        public static string GetAuthorizationUrl(string clientId, string redirectUri, string[] scopes)
        {
            string scope = string.Join(" ", scopes);
            string url = "https://accounts.google.com/o/oauth2/v2/auth" +
                         "?client_id=" + Uri.EscapeDataString(clientId) +
                         "&redirect_uri=" + Uri.EscapeDataString(redirectUri) +
                         "&response_type=code" +
                         "&scope=" + Uri.EscapeDataString(scope) +
                         "&access_type=offline" +
                         "&prompt=consent";
            return url;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Replace these placeholders with real values before running
                string clientId = "YOUR_CLIENT_ID";
                string clientSecret = "YOUR_CLIENT_SECRET";
                string redirectUri = "urn:ietf:wg:oauth:2.0:oob";
                string[] scopes = new string[]
                {
                    "https://mail.google.com/",
                    "https://www.googleapis.com/auth/calendar"
                };

                // Guard against placeholder credentials
                if (clientId.StartsWith("YOUR_") || clientSecret.StartsWith("YOUR_"))
                {
                    Console.Error.WriteLine("Please replace placeholder clientId and clientSecret with real values.");
                    return;
                }

                // Generate the authorization URL
                string authorizationUrl = GoogleOAuthHelper.GetAuthorizationUrl(clientId, redirectUri, scopes);
                Console.WriteLine("Open the following URL in a browser to authorize the application:");
                Console.WriteLine(authorizationUrl);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
