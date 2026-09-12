using Aspose.Email.Clients.Exchange;
using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

namespace GmailTokenRefreshSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Placeholder credentials – replace with real values.
                string clientId = "YOUR_CLIENT_ID";
                string clientSecret = "YOUR_CLIENT_SECRET";
                string refreshToken = "YOUR_REFRESH_TOKEN";
                string defaultEmail = "user@example.com";

                // Skip execution when placeholders are detected to avoid real network calls.
                if (clientId.StartsWith("YOUR_") ||
                    clientSecret.StartsWith("YOUR_") ||
                    refreshToken.StartsWith("YOUR_"))
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping Gmail operations.");
                    return;
                }

                // Obtain a token provider for Google and retrieve the initial access token.
                TokenProvider tokenProvider = TokenProvider.Google.GetInstance(clientId, clientSecret, refreshToken);
                OAuthToken oauthToken;
                try
                {
                    oauthToken = tokenProvider.GetAccessToken();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to acquire access token: {ex.Message}");
                    return;
                }

                // Create the Gmail client using the access token.
                IGmailClient gmailClient;
                try
                {
                    gmailClient = GmailClient.GetInstance(oauthToken.Token, defaultEmail);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create Gmail client: {ex.Message}");
                    return;
                }

                // Use the client within a using block to ensure proper disposal.
                using (gmailClient)
                {
                    try
                    {
                        // Example operation: list messages (placeholder – actual call may be omitted in CI).
                        // ExchangeMessageInfoCollection messages = gmailClient.ListMessages();

                        // Refresh the access token before the next long‑running operation.
                        gmailClient.RefreshToken();

                        // Placeholder for another operation that would benefit from a refreshed token.
                        // var sentMessage = new MailMessage(defaultEmail, defaultEmail, "Test", "Body");
                        // gmailClient.SendMessage(sentMessage);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Gmail operation failed: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
