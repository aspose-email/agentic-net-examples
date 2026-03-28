using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // OAuth2 parameters (replace with real values)
            string clientId = "clientId";
            string clientSecret = "clientSecret";
            string authorizationCode = "authCode";
            string redirectUri = "https://yourapp.example.com/oauth2callback";
            string tokenEndpoint = "https://oauth2.googleapis.com/token";

            if (clientId == "clientId" || clientSecret == "clientSecret" || authorizationCode == "authCode")
            {
                Console.WriteLine("Placeholder OAuth credentials detected. Skipping external token exchange.");
                return;
            }

            // Exchange authorization code for refresh token
            string refreshToken;
            using (HttpClient httpClient = new HttpClient())
            {
                var requestContent = new StringContent(
                    $"code={authorizationCode}&client_id={clientId}&client_secret={clientSecret}&redirect_uri={redirectUri}&grant_type=authorization_code",
                    Encoding.UTF8,
                    "application/x-www-form-urlencoded");

                HttpResponseMessage response = httpClient.PostAsync(tokenEndpoint, requestContent).Result;
                response.EnsureSuccessStatusCode();

                string jsonResponse = response.Content.ReadAsStringAsync().Result;
                using (JsonDocument jsonDoc = JsonDocument.Parse(jsonResponse))
                {
                    JsonElement root = jsonDoc.RootElement;
                    if (root.TryGetProperty("refresh_token", out JsonElement refreshTokenElement))
                    {
                        refreshToken = refreshTokenElement.GetString();
                    }
                    else
                    {
                        Console.Error.WriteLine("Refresh token not found in the response.");
                        return;
                    }
                }
            }

            // Create Gmail client using the obtained refresh token
            string defaultEmail = "user@example.com";
            IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail);
            using (gmailClient)
            {
                // Refresh access token explicitly (optional, as it may happen automatically)
                gmailClient.RefreshToken();

                // Example: list messages (placeholder - actual usage may vary)
                var messages = gmailClient.ListMessages();
                foreach (var messageInfo in messages)
                {
                    // Fetch full message to read subject
                    var mailMessage = gmailClient.FetchMessage(messageInfo.Id);
                    Console.WriteLine(mailMessage.Subject);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
