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
            // Placeholder credentials – replace with real values before running
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string refreshToken = "YOUR_REFRESH_TOKEN";
            string defaultEmail = "user@example.com";

            // Skip execution when placeholders are present to avoid unwanted network calls
            if (clientId.StartsWith("YOUR_") || clientSecret.StartsWith("YOUR_") || refreshToken.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail client initialization.");
                return;
            }

            // Create a token provider for Google and obtain an OAuth token
            TokenProvider tokenProvider = TokenProvider.Google.GetInstance(clientId, clientSecret, refreshToken);
            OAuthToken token = tokenProvider.GetAccessToken();

            // Initialize the Gmail client using the access token
            using (IGmailClient gmailClient = GmailClient.GetInstance(token.Token, defaultEmail))
            {
                // Example placeholder operation – in a real scenario you could list messages, send mail, etc.
                Console.WriteLine("Gmail client initialized successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
