using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace AsposeEmailOAuthSample
{
    class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                // Authorization code obtained from the OAuth2 authorization step
                string authorizationCode = "your-authorization-code";

                // OAuth2 token endpoint (example for Microsoft identity platform)
                string tokenEndpoint = "https://login.microsoftonline.com/common/oauth2/v2.0/token";

                // Client credentials
                string clientId = "your-client-id";
                string clientSecret = "your-client-secret";
                string redirectUri = "your-redirect-uri";

                using (HttpClient httpClient = new HttpClient())
                {
                    var requestData = new Dictionary<string, string>
                    {
                        { "client_id", clientId },
                        { "scope", "https://outlook.office.com/.default offline_access" },
                        { "code", authorizationCode },
                        { "redirect_uri", redirectUri },
                        { "grant_type", "authorization_code" },
                        { "client_secret", clientSecret }
                    };

                    using (HttpContent content = new FormUrlEncodedContent(requestData))
                    {
                        HttpResponseMessage response = await httpClient.PostAsync(tokenEndpoint, content);
                        response.EnsureSuccessStatusCode();

                        string jsonResponse = await response.Content.ReadAsStringAsync();

                        using (JsonDocument jsonDoc = JsonDocument.Parse(jsonResponse))
                        {
                            JsonElement root = jsonDoc.RootElement;
                            if (root.TryGetProperty("refresh_token", out JsonElement refreshTokenElement))
                            {
                                string refreshToken = refreshTokenElement.GetString();
                                Console.WriteLine("Refresh token obtained:");
                                Console.WriteLine(refreshToken);
                            }
                            else
                            {
                                Console.Error.WriteLine("Refresh token not found in the response.");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
