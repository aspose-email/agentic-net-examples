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
            // Placeholder credentials – replace with real values.
            string clientId = "your-client-id";
            string clientSecret = "your-client-secret";
            string refreshToken = "your-refresh-token";
            string defaultEmail = "your-email@gmail.com";

            // Guard against placeholder values to avoid live network calls during CI.
            if (clientId == "your-client-id" ||
                clientSecret == "your-client-secret" ||
                refreshToken == "your-refresh-token" ||
                defaultEmail == "your-email@gmail.com")
            {
                Console.Error.WriteLine("Please provide valid Google OAuth credentials before running the sample.");
                return;
            }

            // Obtain a token provider for Google OAuth.
            TokenProvider tokenProvider = TokenProvider.Google.GetInstance(clientId, clientSecret, refreshToken);

            // Create the SMTP client using the token provider.
            using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587, defaultEmail, tokenProvider, SecurityOptions.Auto))
            {
                try
                {
                    // Prepare a simple email message.
                    MailMessage message = new MailMessage();
                    message.From = defaultEmail;
                    message.To.Add(defaultEmail);
                    message.Subject = "Test Email via Gmail SMTP with OAuth2";
                    message.Body = "This email was sent using Aspose.Email with an OAuth 2.0 access token.";

                    // Send the message.
                    client.Send(message);
                    Console.WriteLine("Message sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error sending email: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
