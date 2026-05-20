using System;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // SMTP server configuration (replace with real values)
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls during CI
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping send operation.");
                return;
            }

            // Create a simple email message
            MailMessage message = new MailMessage();
            message.From = "sender@example.com";
            message.To.Add("recipient@example.com");
            message.Subject = "Test Email with Custom Header";
            message.Body = "This email contains a custom X-Audit-Trail header.";

            // Prepare JSON payload for the custom header
            string jsonPayload = "{\"operation\":\"Send\",\"timestamp\":\"2024-01-01T12:00:00Z\",\"user\":\"sender@example.com\"}";

            // Add the custom X-Audit-Trail header
            message.Headers.Add("X-Audit-Trail", jsonPayload);

            // Send the message using SmtpClient
            using (SmtpClient client = new SmtpClient(host, port, username, password))
            {
                try
                {
                    client.Send(message);
                    Console.WriteLine("Message sent successfully.");
                }
                catch (SmtpException ex)
                {
                    Console.Error.WriteLine($"SMTP error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Unexpected error during send: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Fatal error: {ex.Message}");
        }
    }
}
