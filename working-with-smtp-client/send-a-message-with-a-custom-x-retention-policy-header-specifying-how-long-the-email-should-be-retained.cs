using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder connection settings
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Detect placeholder credentials and skip actual sending
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.WriteLine("Placeholder credentials detected. Skipping email send.");
                return;
            }

            // Create the email message
            MailMessage message = new MailMessage();
            message.From = "sender@example.com";
            message.To.Add("recipient@example.com");
            message.Subject = "Test Message with Retention Header";
            message.Body = "This email includes a custom X‑Retention‑Policy header.";

            // Add custom X‑Retention‑Policy header (e.g., retain for 30 days)
            message.Headers.Add("X-Retention-Policy", "30 days");

            // Send the message using SMTP client
            try
            {
                using (SmtpClient client = new SmtpClient(host, port))
                {
                    client.Username = username;
                    client.Password = password;
                    client.SecurityOptions = SecurityOptions.Auto;
                    client.Send(message);
                    Console.WriteLine("Email sent successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to send email: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
