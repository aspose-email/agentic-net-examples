using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

namespace Sample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder SMTP settings – skip actual send in CI environments
                string host = "smtp.example.com";
                string username = "user@example.com";
                string password = "password";

                if (host.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder SMTP settings detected. Skipping send.");
                    return;
                }

                // Create SMTP client with SSL implicit encryption (entire session encrypted)
                using (SmtpClient client = new SmtpClient(host, 465, username, password, SecurityOptions.SSLImplicit))
                {
                    try
                    {
                        // Validate credentials before sending
                        client.ValidateCredentials();

                        // Build a simple email message
                        MailMessage message = new MailMessage();
                        message.From = username;
                        message.To.Add("recipient@example.com");
                        message.Subject = "Test Email over SSL";
                        message.Body = "This email is sent using an SSL-encrypted SMTP session.";

                        // Send the message
                        client.Send(message);
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
