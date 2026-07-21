using System;
using Aspose.Email;
using Aspose.Email.Clients;

class Program
{
    static void Main()
    {
        try
        {
            // Define the OAuth token endpoint and client credentials.
            const string tenantId = "YOUR_TENANT_ID";
            const string requestUrl = "https://login.microsoftonline.com/" + tenantId + "/oauth2/v2.0/token";
            const string clientId = "YOUR_CLIENT_ID";
            const string clientSecret = "YOUR_CLIENT_SECRET";
            const string refreshToken = ""; // Not used in client‑credentials flow; keep empty.

            // Guard against placeholder values.
            if (string.IsNullOrWhiteSpace(tenantId) || tenantId.Contains("YOUR_"))
                throw new InvalidOperationException("Please replace YOUR_TENANT_ID with a valid Azure AD tenant ID.");
            if (string.IsNullOrWhiteSpace(clientId) || clientId.Contains("YOUR_"))
                throw new InvalidOperationException("Please replace YOUR_CLIENT_ID with a valid client ID.");
            if (string.IsNullOrWhiteSpace(clientSecret) || clientSecret.Contains("YOUR_"))
                throw new InvalidOperationException("Please replace YOUR_CLIENT_SECRET with a valid client secret.");

            // Create the token provider (IDisposable) and retrieve the access token.
            using (TokenProvider tokenProvider = TokenProvider.GetInstance(requestUrl, clientId, clientSecret, refreshToken))
            {
                // GetAccessToken may return a string or an OAuthToken object.
                object tokenResult = tokenProvider.GetAccessToken();

                string accessToken;
                if (tokenResult is string str)
                {
                    accessToken = str;
                }
                else if (tokenResult is Aspose.Email.Clients.OAuthToken oauth)
                {
                    accessToken = oauth.Token;
                }
                else
                {
                    accessToken = tokenResult?.ToString() ?? string.Empty;
                }

                Console.WriteLine("Access Token: " + accessToken);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
