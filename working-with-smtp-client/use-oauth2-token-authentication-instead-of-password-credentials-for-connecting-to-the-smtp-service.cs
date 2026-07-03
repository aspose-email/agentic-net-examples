using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // SMTP server configuration
            string smtpHost = "smtp.example.com";
            int smtpPort = 587;
            string smtpUser = "user@example.com";

            // OAuth2 access token (replace with a real token)
            string accessToken = "YOUR_ACCESS_TOKEN";

            // Guard against placeholder credentials
            if (string.IsNullOrWhiteSpace(accessToken) || accessToken.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("OAuth token is missing or placeholder. Skipping SMTP send.");
                return;
            }

            // Create a simple email message
            MailMessage message = new MailMessage();
            message.From = new MailAddress(smtpUser);
            message.To.Add(new MailAddress("recipient@example.com"));
            message.Subject = "Test Email via OAuth2 SMTP";
            message.Body = "This email was sent using Aspose.Email with OAuth2 authentication.";

            // Send the email using OAuth2 token authentication
            try
            {
                using (SmtpClient client = new SmtpClient(smtpHost, smtpPort, smtpUser, accessToken, true, SecurityOptions.Auto))
                {
                    client.Send(message);
                }

                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to send email: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
