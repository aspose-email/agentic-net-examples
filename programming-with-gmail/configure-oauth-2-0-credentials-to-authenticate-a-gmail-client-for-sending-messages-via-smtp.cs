using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Google;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // OAuth 2.0 credentials (replace with real values)
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string refreshToken = "YOUR_REFRESH_TOKEN";
            string defaultEmail = "user@example.com";

            // Skip execution if placeholder credentials are detected
            if (clientId.StartsWith("YOUR_") || clientSecret.StartsWith("YOUR_") || refreshToken.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Please provide valid OAuth 2.0 credentials before running the sample.");
                return;
            }

            // Obtain a token provider for Google OAuth
            TokenProvider tokenProvider = TokenProvider.Google.GetInstance(clientId, clientSecret, refreshToken);

            // Create a Gmail client instance (optional, shown for completeness)
            IGmailClient gmailClient = null;
            try
            {
                gmailClient = GmailClient.GetInstance(clientId, clientSecret, refreshToken, defaultEmail);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create Gmail client: {ex.Message}");
                return;
            }

            // Configure SMTP client to use OAuth 2.0 token
            using (SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587, defaultEmail, tokenProvider, SecurityOptions.Auto))
            {
                // Build the email message
                MailMessage message = new MailMessage();
                message.From = defaultEmail;
                message.To.Add(defaultEmail);
                message.Subject = "Test email via Gmail SMTP with OAuth2";
                message.Body = "Hello, this is a test message sent using Aspose.Email with OAuth2 authentication.";

                try
                {
                    smtpClient.Send(message);
                    Console.WriteLine("Message sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to send email: {ex.Message}");
                }
            }

            // Dispose Gmail client if it was created
            gmailClient?.Dispose();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
