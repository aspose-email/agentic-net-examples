using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Aspose.Email.Clients;

class Program
{
    static void Main()
    {
        try
        {
            // OAuth2 parameters (replace with actual values)
            string clientId = "your_client_id";
            string clientSecret = "your_client_secret";
            string authorizationCode = "your_authorization_code";
            string redirectUri = "your_redirect_uri";
            string tokenEndpoint = "https://login.microsoftonline.com/common/oauth2/v2.0/token";

            // Exchange the authorization code for a refresh token
            string refreshToken = ExchangeCodeForRefreshToken(tokenEndpoint, clientId, clientSecret, authorizationCode, redirectUri).GetAwaiter().GetResult();

            Console.WriteLine("Refresh Token: " + refreshToken);

            // Create a TokenProvider using the obtained refresh token
            Aspose.Email.Clients.TokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Outlook.GetInstance(clientId, clientSecret, refreshToken);

            // Obtain an access token (the token is managed internally by the provider)
            tokenProvider.GetAccessToken();

            Console.WriteLine("Access token obtained successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }

    private static async Task<string> ExchangeCodeForRefreshToken(string tokenEndpoint, string clientId, string clientSecret, string code, string redirectUri)
    {
        using (HttpClient httpClient = new HttpClient())
        {
            List<KeyValuePair<string, string>> parameters = new List<KeyValuePair<string, string>>();
            parameters.Add(new KeyValuePair<string, string>("client_id", clientId));
            parameters.Add(new KeyValuePair<string, string>("client_secret", clientSecret));
            parameters.Add(new KeyValuePair<string, string>("code", code));
            parameters.Add(new KeyValuePair<string, string>("redirect_uri", redirectUri));
            parameters.Add(new KeyValuePair<string, string>("grant_type", "authorization_code"));

            using (FormUrlEncodedContent content = new FormUrlEncodedContent(parameters))
            {
                HttpResponseMessage response = await httpClient.PostAsync(tokenEndpoint, content);
                response.EnsureSuccessStatusCode();

                string json = await response.Content.ReadAsStringAsync();

                using (JsonDocument doc = JsonDocument.Parse(json))
                {
                    if (doc.RootElement.TryGetProperty("refresh_token", out JsonElement refreshElement))
                    {
                        return refreshElement.GetString();
                    }
                    else
                    {
                        throw new Exception("Refresh token not found in the response.");
                    }
                }
            }
        }
    }
}