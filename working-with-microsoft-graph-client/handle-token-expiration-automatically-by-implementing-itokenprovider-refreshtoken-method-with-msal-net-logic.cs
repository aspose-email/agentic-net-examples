using System;
using System.Collections.Generic;
using System.Net.Http;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Exchange;
using Aspose.Email.Clients.Graph;

namespace AsposeEmailGraphSample
{
    // Custom token provider implementing Aspose.Email.Clients.ITokenProvider
    public class MyTokenProvider : Aspose.Email.Clients.ITokenProvider
    {
        private readonly string _clientId;
        private readonly string _clientSecret;
        private readonly string _refreshToken;
        private readonly string _tokenEndpoint;
        private string _accessToken;
        private DateTime _expiresOn;

        public MyTokenProvider(string clientId, string clientSecret, string refreshToken, string tokenEndpoint)
        {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _refreshToken = refreshToken;
            _tokenEndpoint = tokenEndpoint;
        }

        // Returns OAuthToken as required by Aspose.Email.Clients.ITokenProvider
        public OAuthToken GetAccessToken()
        {
            if (string.IsNullOrEmpty(_accessToken) || DateTime.UtcNow >= _expiresOn)
            {
                RefreshTokenInternal();
            }
            return new OAuthToken(_accessToken, _expiresOn);
        }

        // Overload that forces a refresh when requested
        public OAuthToken GetAccessToken(bool forceRefresh)
        {
            if (forceRefresh)
            {
                RefreshTokenInternal();
            }
            else if (string.IsNullOrEmpty(_accessToken) || DateTime.UtcNow >= _expiresOn)
            {
                RefreshTokenInternal();
            }
            return new OAuthToken(_accessToken, _expiresOn);
        }

        // Refreshes the access token using MSAL‑like HTTP request
        private void RefreshTokenInternal()
        {
            using (var httpClient = new HttpClient())
            {
                var parameters = new Dictionary<string, string>
                {
                    { "client_id", _clientId },
                    { "client_secret", _clientSecret },
                    { "refresh_token", _refreshToken },
                    { "grant_type", "refresh_token" },
                    { "scope", "https://graph.microsoft.com/.default" }
                };

                var content = new FormUrlEncodedContent(parameters);
                HttpResponseMessage response = httpClient.PostAsync(_tokenEndpoint, content).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();

                var json = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

                // Simple parsing – in real code use a JSON library
                var tokenStart = json.IndexOf("\"access_token\":\"", StringComparison.Ordinal) + 16;
                var tokenEnd = json.IndexOf("\"", tokenStart, StringComparison.Ordinal);
                _accessToken = json.Substring(tokenStart, tokenEnd - tokenStart);

                var expiresStart = json.IndexOf("\"expires_in\":", StringComparison.Ordinal) + 13;
                var expiresEnd = json.IndexOf(",", expiresStart, StringComparison.Ordinal);
                var expiresInSec = int.Parse(json.Substring(expiresStart, expiresEnd - expiresStart));
                _expiresOn = DateTime.UtcNow.AddSeconds(expiresInSec - 60); // buffer before actual expiry
            }
        }

        public void Dispose()
        {
            // No unmanaged resources to release
        }
    }

    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder credentials – replace with real values or guard against execution
                const string clientId = "YOUR_CLIENT_ID";
                const string clientSecret = "YOUR_CLIENT_SECRET";
                const string refreshToken = "YOUR_REFRESH_TOKEN";
                const string tokenEndpoint = "https://login.microsoftonline.com/common/oauth2/v2.0/token";
                const string tenantId = "YOUR_TENANT_ID";

                // Early exit if placeholders are not replaced
                if (clientId.StartsWith("YOUR_") || clientSecret.StartsWith("YOUR_") ||
                    refreshToken.StartsWith("YOUR_") || tenantId.StartsWith("YOUR_"))
                {
                    Console.Error.WriteLine("Please provide valid client credentials before running the sample.");
                    return;
                }

                // Initialize custom token provider
                using (Aspose.Email.Clients.ITokenProvider tokenProvider = new MyTokenProvider(clientId, clientSecret, refreshToken, tokenEndpoint))
                {
                    // Create Graph client
                    using (IGraphClient client = GraphClient.GetClient(tokenProvider, tenantId))
                    {
                        // List messages from the Inbox using the supported overload
                        MessageInfoCollection messages = client.ListMessages(KnownFolders.Inbox, null);
                        Console.WriteLine($"Total messages in Inbox: {messages?.Count ?? 0}");
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
