using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

namespace AsposeEmailGmailSample
{
    public class GoogleUser : IDisposable
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _refreshToken;
        private readonly string _defaultEmail;
        private IGmailClient _gmailClient;

        public GoogleUser(string clientId, string clientSecret, string refreshToken, string defaultEmail)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _refreshToken = refreshToken;
            _defaultEmail = defaultEmail;
        }

        public bool Authenticate()
        {
            try
            {
                // Acquire a token provider for Google (not directly used here but shown for completeness)
                Aspose.Email.Clients.TokenProvider tokenProvider = Aspose.Email.Clients.TokenProvider.Google.GetInstance(_clientId, _clientSecret, _refreshToken);

                // Create Gmail client instance using the refresh token flow
                _gmailClient = GmailClient.GetInstance(_clientId, _clientSecret, _refreshToken, _defaultEmail);
                return true;
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Authentication failed: " + ex.Message);
                return false;
            }
        }

        public void RefreshToken()
        {
            if (_gmailClient == null)
            {
                Console.Error.WriteLine("Client not initialized.");
                return;
            }

            try
            {
                _gmailClient.RefreshToken();
                Console.WriteLine("Access token refreshed successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Token refresh failed: " + ex.Message);
            }
        }

        public void PrintUserProfile()
        {
            if (_gmailClient == null)
            {
                Console.Error.WriteLine("Client not initialized.");
                return;
            }

            try
            {
                // Retrieve user settings as a simple profile representation
                Dictionary<string, string> settings = _gmailClient.GetSettings();
                foreach (KeyValuePair<string, string> kvp in settings)
                {
                    Console.WriteLine($"{kvp.Key}: {kvp.Value}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Failed to retrieve user profile: " + ex.Message);
            }
        }

        public void Dispose()
        {
            if (_gmailClient != null)
            {
                _gmailClient.Dispose();
                _gmailClient = null;
            }
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            try
            {
                // Replace the placeholders with actual credentials
                string clientId = "YOUR_CLIENT_ID";
                string clientSecret = "YOUR_CLIENT_SECRET";
                string refreshToken = "YOUR_REFRESH_TOKEN";
                string defaultEmail = "user@example.com";

                using (GoogleUser googleUser = new GoogleUser(clientId, clientSecret, refreshToken, defaultEmail))
                {
                    if (googleUser.Authenticate())
                    {
                        Console.WriteLine("Authenticated successfully.");
                        Console.WriteLine("User profile settings:");
                        googleUser.PrintUserProfile();

                        Console.WriteLine("Refreshing access token...");
                        googleUser.RefreshToken();

                        Console.WriteLine("User profile settings after refresh:");
                        googleUser.PrintUserProfile();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Unhandled exception: " + ex.Message);
            }
        }
    }
}