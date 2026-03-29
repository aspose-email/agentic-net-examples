using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Imap;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Clients.Google;

namespace AsposeEmailOAuthExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder OAuth credentials
                string clientId = "clientId";
                string clientSecret = "clientSecret";
                string refreshToken = "refreshToken";
                string userEmail = "user@example.com";

                // Placeholder server settings
                string imapHost = "imap.example.com";
                string smtpHost = "smtp.example.com";
                string username = "user@example.com";

                // Guard against executing real network calls with placeholder data
                if (imapHost.Contains("example.com") || smtpHost.Contains("example.com"))
                {
                    Console.WriteLine("Placeholder credentials detected. Skipping network operations.");
                    return;
                }

                // Obtain a TokenProvider for Google (Gmail) OAuth
                TokenProvider tokenProvider = TokenProvider.Google.GetInstance(clientId, clientSecret, refreshToken);

                // Acquire an access token (optional, for demonstration)
                OAuthToken token = tokenProvider.GetAccessToken();
                Console.WriteLine($"Access Token: {token.Token}");

                // Configure IMAP client with OAuth token provider
                using (ImapClient imapClient = new ImapClient(imapHost, username, tokenProvider))
                {
                    try
                    {
                        // The client automatically uses the token provider for authentication
                        // Example operation: list message IDs
                        var messages = imapClient.ListMessages();
                        Console.WriteLine($"Total messages: {messages.Count}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"IMAP operation failed: {ex.Message}");
                    }
                }

                // Configure SMTP client with OAuth token provider
                using (SmtpClient smtpClient = new SmtpClient(smtpHost, username, tokenProvider))
                {
                    try
                    {
                        // Example operation: send a simple email
                        MailMessage message = new MailMessage
                        {
                            From = userEmail,
                            To = userEmail,
                            Subject = "OAuth Test",
                            Body = "This email was sent using OAuth authentication."
                        };

                        smtpClient.Send(message);
                        Console.WriteLine("Email sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"SMTP operation failed: {ex.Message}");
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
