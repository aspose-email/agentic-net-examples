using System;
using Aspose.Email;
using Aspose.Email.Clients.Google;

namespace AsposeEmailGmailExample
{
    // Author: Aspose.Email example demonstrating Gmail client creation with OAuth credentials.
    class Program
    {
        static void Main()
        {
            try
            {
                // Replace the placeholders with actual values from Google Cloud Console.
                string clientId = "YOUR_CLIENT_ID";
                string clientSecret = "YOUR_CLIENT_SECRET";
                string refreshToken = "YOUR_REFRESH_TOKEN";
                string defaultEmail = "YOUR_EMAIL_ADDRESS";

                // Guard against placeholder values.
                if (string.IsNullOrWhiteSpace(clientId) || clientId.StartsWith("YOUR_") ||
                    string.IsNullOrWhiteSpace(clientSecret) || clientSecret.StartsWith("YOUR_") ||
                    string.IsNullOrWhiteSpace(refreshToken) || refreshToken.StartsWith("YOUR_") ||
                    string.IsNullOrWhiteSpace(defaultEmail) || defaultEmail.StartsWith("YOUR_"))
                {
                    Console.Error.WriteLine("Please replace the placeholder strings with valid Google OAuth credentials.");
                    return;
                }

                // Create Gmail client instance.
                using (IGmailClient gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail))
                {
                    // Simple operation: output the default email address to verify the client is functional.
                    Console.WriteLine($"Gmail client created for: {gmailClient.DefaultEmail}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
