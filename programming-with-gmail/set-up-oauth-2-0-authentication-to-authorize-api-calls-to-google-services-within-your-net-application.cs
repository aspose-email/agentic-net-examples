using System;
using Aspose.Email;

using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;

namespace GmailOAuthExample
{
    class Program
    {
        static void Main()
        {
            // Replace the placeholder values with your actual Google OAuth credentials.
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string refreshToken = "YOUR_REFRESH_TOKEN";
            string defaultEmail = "YOUR_EMAIL@example.com";

            // Guard against placeholder values.
            if (clientId.StartsWith("YOUR_") ||
                clientSecret.StartsWith("YOUR_") ||
                refreshToken.StartsWith("YOUR_") ||
                defaultEmail.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Please replace placeholder credentials with valid values.");
                return;
            }

            try
            {
                // Obtain a Google token provider.
                Aspose.Email.Clients.ITokenProvider tokenProvider = TokenProvider.Google.GetInstance(clientId, clientSecret, refreshToken);

                // Retrieve the OAuth token and extract the access token string.
                OAuthToken oauthToken = tokenProvider.GetAccessToken();
                string accessToken = oauthToken.Token;

                // Create the Gmail client using the access token.
                using (IGmailClient gmailClient = GmailClient.GetInstance(accessToken, defaultEmail))
                {
                    // Build a simple email message.
                    MailMessage message = new MailMessage
                    {
                        From = new MailAddress(defaultEmail),
                        Subject = "Aspose.Email Gmail OAuth Test",
                        Body = "This email was sent using Aspose.Email with Google OAuth 2.0 authentication."
                    };
                    message.To.Add(new MailAddress(defaultEmail));

                    // Send the message.
                    gmailClient.SendMessage(message);
                    Console.WriteLine("Email sent successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
