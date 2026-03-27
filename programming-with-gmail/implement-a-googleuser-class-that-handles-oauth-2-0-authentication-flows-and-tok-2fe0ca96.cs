using System;
using System.Net.Http;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

namespace AsposeEmailSample
{
    // Handles Gmail OAuth authentication, token refresh, and user profile retrieval.
    public class GoogleUser : IDisposable
    {
        private readonly IGmailClient _gmailClient;
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _refreshToken;
        private readonly string _defaultEmail;
        private bool _disposed;

        public GoogleUser(string clientId, string clientSecret, string refreshToken, string defaultEmail)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _refreshToken = refreshToken;
            _defaultEmail = defaultEmail;

            // Create Gmail client instance using OAuth credentials.
            _gmailClient = GmailClient.GetInstance(_clientId, _clientSecret, _refreshToken, _defaultEmail);
        }

        // Refreshes the access token using the Gmail client.
        public void RefreshAccessToken()
        {
            _gmailClient.RefreshToken();
        }

        // Retrieves the current access token.
        public string GetAccessToken()
        {
            return _gmailClient.AccessToken;
        }

        // Retrieves the user's Google profile using the access token.
        public string GetUserProfile()
        {
            string accessToken = GetAccessToken();
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
                HttpResponseMessage response = httpClient.GetAsync("https://www.googleapis.com/oauth2/v1/userinfo?alt=json").Result;
                response.EnsureSuccessStatusCode();
                return response.Content.ReadAsStringAsync().Result;
            }
        }

        // Exposes the underlying Gmail client for additional operations if needed.
        public IGmailClient Client => _gmailClient;

        public void Dispose()
        {
            if (!_disposed)
            {
                _gmailClient.Dispose();
                _disposed = true;
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Dummy OAuth credentials – replace with real values.
                const string clientId = "your-client-id";
                const string clientSecret = "your-client-secret";
                const string refreshToken = "your-refresh-token";
                const string defaultEmail = "user@example.com";

                using (GoogleUser googleUser = new GoogleUser(clientId, clientSecret, refreshToken, defaultEmail))
                {
                    // Refresh the access token.
                    googleUser.RefreshAccessToken();

                    // Output the current access token.
                    Console.WriteLine("Access Token: " + googleUser.GetAccessToken());

                    // Retrieve and display the user profile information.
                    string profileJson = googleUser.GetUserProfile();
                    Console.WriteLine("User Profile:");
                    Console.WriteLine(profileJson);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
            }
        }
    }
}
