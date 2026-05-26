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
            // Placeholder credentials – skip real network call in CI environments
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder SMTP settings detected. Skipping send operation.");
                return;
            }

            // Create a simple email message
            MailMessage message = new MailMessage
            {
                From = "sender@example.com",
                To = "recipient@example.com",
                Subject = "Test Email",
                Body = "This is a test email sent using Aspose.Email with SSL/TLS."
            };

            // Initialize SmtpClient with SSL/TLS explicit mode.
            // Use numeric cast to avoid reliance on enum member name that may differ across library versions.
            using (SmtpClient client = new SmtpClient(host, port, username, password, (SecurityOptions)1))
            {
                try
                {
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
