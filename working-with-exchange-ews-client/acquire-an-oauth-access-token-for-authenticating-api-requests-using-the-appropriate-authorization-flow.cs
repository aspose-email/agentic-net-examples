using Aspose.Email;
using System;
using Aspose.Email.Clients;

// Author: Generated example for acquiring an OAuth access token using Aspose.Email TokenProvider.
class Program
{
    static void Main()
    {
        try
        {
            // OAuth parameters – replace with your actual values.
            string requestUrl = "https://login.microsoftonline.com/common/oauth2/v2.0/token";
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";

            // Create an Outlook token provider instance.
            TokenProvider tokenProvider = TokenProvider.GetInstance(requestUrl, clientId, clientSecret, refreshToken);

            // Retrieve the access token.
            Aspose.Email.Clients.OAuthToken accessToken = tokenProvider.GetAccessToken();

            // Display the token string.
            Console.WriteLine("Access Token: " + accessToken.Token);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error acquiring token: " + ex.Message);
        }
    }
}
