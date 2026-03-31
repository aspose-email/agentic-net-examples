using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – in real scenarios replace with actual values.
            string clientId = "clientId";
            string clientSecret = "clientSecret";
            string refreshToken = "refreshToken";
            string defaultEmail = "user@example.com";

            // Guard against executing live network calls with placeholder credentials.
            if (clientId == "clientId" || clientSecret == "clientSecret" ||
                refreshToken == "refreshToken")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail client operations.");
                return;
            }

            // Create Gmail client instance.
            using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail))
            {
                // Retrieve current configuration settings.
                string currentAccessToken = gmailClient.AccessToken;
                string currentDefaultEmail = gmailClient.DefaultEmail;
                int currentTimeout = gmailClient.Timeout;
                Console.WriteLine($"Current Access Token: {currentAccessToken}");
                Console.WriteLine($"Current Default Email: {currentDefaultEmail}");
                Console.WriteLine($"Current Timeout (ms): {currentTimeout}");

                // Modify configuration settings.
                gmailClient.Timeout = 200000; // Increase timeout to 200 seconds.
                // Example: assign a proxy if needed (null here means no proxy).
                gmailClient.Proxy = null;

                // Refresh the access token programmatically.
                try
                {
                    gmailClient.RefreshToken();
                    Console.WriteLine("Access token refreshed successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to refresh token: {ex.Message}");
                }

                // Verify modified settings.
                Console.WriteLine($"Updated Timeout (ms): {gmailClient.Timeout}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
