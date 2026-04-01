using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

namespace AsposeEmailGmailSample
{
    // Handles OAuth 2.0 flows and token management for a Google user.
    public class GoogleUser : IDisposable
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _refreshToken;
        private readonly string _defaultEmail;
        private readonly TokenProvider _tokenProvider;
        private readonly IGmailClient _gmailClient;

        public GoogleUser(string clientId, string clientSecret, string refreshToken, string defaultEmail)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _refreshToken = refreshToken;
            _defaultEmail = defaultEmail;

            // Guard against placeholder credentials to avoid live network calls.
            if (string.IsNullOrEmpty(_clientId) || _clientId == "clientId" ||
                string.IsNullOrEmpty(_clientSecret) || _clientSecret == "clientSecret" ||
                string.IsNullOrEmpty(_refreshToken) || _refreshToken == "refreshToken")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Gmail client will not be initialized.");
                return;
            }

            try
            {
                _tokenProvider = TokenProvider.Google.GetInstance(_clientId, _clientSecret, _refreshToken);
                OAuthToken token = _tokenProvider.GetAccessToken();
                _gmailClient = GmailClient.GetInstance(token.Token, _defaultEmail);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to initialize Gmail client: {ex.Message}");
            }
        }

        // Acquires a fresh access token.
        public string AcquireToken()
        {
            if (_tokenProvider == null)
            {
                Console.Error.WriteLine("Token provider not initialized.");
                return null;
            }

            try
            {
                OAuthToken token = _tokenProvider.GetAccessToken();
                return token.Token;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error acquiring token: {ex.Message}");
                return null;
            }
        }

        // Refreshes the access token using the Gmail client.
        public void RefreshAccessToken()
        {
            if (_gmailClient == null)
            {
                Console.Error.WriteLine("Gmail client not initialized.");
                return;
            }

            try
            {
                _gmailClient.RefreshToken();
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error refreshing token: {ex.Message}");
            }
        }

        // Retrieves basic user profile information (placeholder implementation).
        public string GetUserProfile()
        {
            if (_gmailClient == null)
            {
                Console.Error.WriteLine("Gmail client not initialized.");
                return null;
            }

            try
            {
                // Using GetSetting as a placeholder for profile retrieval.
                string profile = _gmailClient.GetSetting("profile");
                return profile;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error retrieving user profile: {ex.Message}");
                return null;
            }
        }

        // Dispose pattern for IDisposable resources.
        public void Dispose()
        {
            if (_gmailClient != null)
            {
                _gmailClient.Dispose();
            }

            if (_tokenProvider != null)
            {
                _tokenProvider.Dispose();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Initialize GoogleUser with placeholder credentials.
                using (GoogleUser user = new GoogleUser("clientId", "clientSecret", "refreshToken", "user@example.com"))
                {
                    string accessToken = user.AcquireToken();
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        Console.WriteLine("Access Token: " + accessToken);
                    }

                    // Refresh the token if needed.
                    user.RefreshAccessToken();

                    // Retrieve and display user profile information.
                    string profile = user.GetUserProfile();
                    if (!string.IsNullOrEmpty(profile))
                    {
                        Console.WriteLine("User Profile: " + profile);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
            }
        }
    }
}
