using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

namespace SmtpFallbackExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder SMTP server details
                string host = "smtp.example.com";
                int port = 587;
                string username = "user@example.com";
                string password = "password";

                // Skip real network calls when placeholder data is used
                if (host.Contains("example.com"))
                {
                    Console.Error.WriteLine("Placeholder SMTP server detected. Skipping connection.");
                    return;
                }

                // Create the client and ensure it is disposed properly
                using (SmtpClient client = new SmtpClient(host, port, username, password, SecurityOptions.Auto))
                {
                    // Allow plain authentication as a fallback if CRAM‑MD5 is not supported
                    client.AllowedAuthentication = SmtpKnownAuthenticationType.CramMD5 | SmtpKnownAuthenticationType.Plain;

                    // Validate credentials (attempts authentication)
                    try
                    {
                        bool isValid = client.ValidateCredentials();
                        Console.WriteLine(isValid ? "Authentication succeeded." : "Authentication failed.");
                    }
                    catch (SmtpException ex)
                    {
                        Console.Error.WriteLine($"SMTP error: {ex.Message}");
                        return;
                    }

                    // Create a simple email message
                    MailMessage message = new MailMessage(username, "recipient@example.com", "Test", "This is a test email.");

                    // Send the message
                    try
                    {
                        client.Send(message);
                        Console.WriteLine("Message sent successfully.");
                    }
                    catch (SmtpException ex)
                    {
                        Console.Error.WriteLine($"Failed to send email: {ex.Message}");
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
