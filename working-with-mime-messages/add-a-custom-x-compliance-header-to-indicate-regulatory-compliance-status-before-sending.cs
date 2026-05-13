using System;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder SMTP configuration
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Skip sending when placeholder values are detected
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP settings detected. Skipping send operation.");
                return;
            }

            using (SmtpClient client = new SmtpClient(host, port, username, password))
            {
                try
                {
                    using (MailMessage message = new MailMessage())
                    {
                        message.From = new MailAddress("sender@example.com");
                        message.To.Add(new MailAddress("recipient@example.com"));
                        message.Subject = "Test Email with Custom Header";
                        message.Body = "This email contains a custom X-Compliance header.";

                        // Add custom X-Compliance header
                        message.Headers.Add("X-Compliance", "Regulated");

                        client.Send(message);
                        Console.WriteLine("Email sent successfully.");
                    }
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
