using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Mime;

namespace AsposeEmailOAuthSample
{
    class Program
    {
        static void Main()
        {
            // OAuth client credentials (replace with real values)
            const string clientId = "YOUR_CLIENT_ID";
            const string clientSecret = "YOUR_CLIENT_SECRET";
            const string refreshToken = "YOUR_REFRESH_TOKEN";
            const string smtpHost = "smtp.example.com";
            const int smtpPort = 587;
            const string smtpUser = "user@example.com";

            // Guard: skip execution when placeholders are still present
            if (clientId.StartsWith("YOUR_") ||
                clientSecret.StartsWith("YOUR_") ||
                refreshToken.StartsWith("YOUR_") ||
                smtpHost.StartsWith("YOUR_") ||
                smtpUser.StartsWith("YOUR_"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping email send.");
                return;
            }

            try
            {
                // Obtain a token provider for Outlook (OAuth 2.0)
                Aspose.Email.Clients.ITokenProvider tokenProvider = TokenProvider.GetInstance(clientId, clientSecret, refreshToken, string.Empty);

                // Retrieve the OAuth token
                OAuthToken oauthToken = tokenProvider.GetAccessToken();

                // Extract the raw access token string
                string accessToken = oauthToken.Token;

                // Initialize the SMTP client with OAuth token
                using (SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort, smtpUser, accessToken, true, SecurityOptions.Auto))
                {
                    // Create a simple email message
                    MailMessage message = new MailMessage
                    {
                        From = new MailAddress(smtpUser),
                        Subject = "Test Email via OAuth2",
                        Body = "This email was sent using Aspose.Email with OAuth 2.0 authentication."
                    };
                    message.To.Add(new MailAddress("recipient@example.com"));

                    // Send the message
                    smtpClient.Send(message);
                    Console.WriteLine("Email sent successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
