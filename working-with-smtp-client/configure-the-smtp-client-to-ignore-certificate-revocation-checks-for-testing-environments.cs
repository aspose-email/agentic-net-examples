using Aspose.Email.Clients;
using System;
using System.Net.Security;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // SMTP server configuration (placeholder values)
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Skip actual network call when placeholder credentials are used
            if (host.Contains("example.com"))
            {
                Console.WriteLine("Placeholder SMTP host detected. Skipping connection and send.");
                return;
            }

            // Initialize SmtpClient with a certificate validation callback that always returns true
            using (SmtpClient client = new SmtpClient(
                host,
                port,
                username,
                password,
                (object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                 System.Security.Cryptography.X509Certificates.X509Chain chain, SslPolicyErrors sslPolicyErrors) => true))
            {
                // Optional: set security options as needed
                client.SecurityOptions = SecurityOptions.Auto;

                // Validate credentials safely
                try
                {
                    client.ValidateCredentials();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Credential validation failed: {ex.Message}");
                    return;
                }

                // Create a simple email message
                using (MailMessage message = new MailMessage())
                {
                    message.From = "sender@example.com";
                    message.To.Add("recipient@example.com");
                    message.Subject = "Test Email";
                    message.Body = "This is a test email sent with certificate revocation checks disabled.";

                    // Send the message
                    try
                    {
                        client.Send(message);
                        Console.WriteLine("Email sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to send email: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
