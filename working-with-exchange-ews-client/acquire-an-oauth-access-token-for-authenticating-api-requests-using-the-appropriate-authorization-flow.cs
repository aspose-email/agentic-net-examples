using System;
using Aspose.Email.Clients;

namespace Example
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // OAuth credentials (replace with actual values)
                string clientId = "your-client-id";
                string clientSecret = "your-client-secret";
                string refreshToken = "your-refresh-token";

                // Acquire an access token using Outlook token provider
                using (Aspose.Email.Clients.TokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken))
                {
                    Aspose.Email.Clients.OAuthToken token = tokenProvider.GetAccessToken();
                    Console.WriteLine("OAuth access token acquired successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}