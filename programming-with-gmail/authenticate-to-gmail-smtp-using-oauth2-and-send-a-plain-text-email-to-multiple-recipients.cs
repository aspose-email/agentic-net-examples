using System;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Clients.Google;
using Aspose.Email.Clients;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";
            string userEmail = "user@example.com";

            if (string.IsNullOrWhiteSpace(clientId) || clientId.StartsWith("your-") ||
                string.IsNullOrWhiteSpace(clientSecret) || clientSecret.StartsWith("your-") ||
                string.IsNullOrWhiteSpace(refreshToken) || refreshToken.StartsWith("your-") ||
                string.IsNullOrWhiteSpace(userEmail) || userEmail.StartsWith("user@"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping Gmail SMTP send.");
                return;
            }

            // Obtain a token provider for Google OAuth2.
            Aspose.Email.Clients.ITokenProvider tokenProvider = TokenProvider.Google.GetInstance(clientId, clientSecret, refreshToken);

            // Create the SMTP client for Gmail using OAuth2 token provider.
            using (SmtpClient client = new SmtpClient("smtp.gmail.com", userEmail, tokenProvider))
            {
                try
                {
                    // Build the email message.
                    MailMessage message = new MailMessage();
                    message.From = userEmail;
                    // Add multiple recipients.
                    message.To.Add("recipient1@example.com");
                    message.To.Add("recipient2@example.com");
                    message.Subject = "Test Email via Gmail SMTP OAuth2";
                    message.Body = "This is a plain text email sent using Aspose.Email with OAuth2 authentication.";

                    // Send the message.
                    client.Send(message);
                    Console.WriteLine("Email sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error sending email: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
