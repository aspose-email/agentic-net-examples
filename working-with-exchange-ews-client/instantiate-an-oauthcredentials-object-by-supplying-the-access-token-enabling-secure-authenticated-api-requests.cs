using System;
using Aspose.Email.Clients;

class Program
{
    static void Main()
    {
        try
        {
            // Supply your OAuth 2.0 access token here
            string accessToken = "your_access_token";

            // Instantiate OAuth credentials using the access token
            OAuthNetworkCredential credentials = new OAuthNetworkCredential(accessToken);

            Console.WriteLine("OAuth credentials instantiated successfully.");
            Console.WriteLine($"Access Token: {credentials.AccessToken}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
