using System;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder SMTP configuration – replace with real values.
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Skip actual sending when placeholders are detected.
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping send operation.");
                return;
            }

            // Create the mail message.
            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.From = "sender@example.com";
                mailMessage.To.Add("recipient@example.com");
                mailMessage.Subject = "Test Email with Correlation ID";
                mailMessage.Body = "This email contains a custom X-Correlation-ID header.";

                // Add custom X-Correlation-ID header.
                string correlationId = Guid.NewGuid().ToString();
                mailMessage.Headers.Add("X-Correlation-ID", correlationId);
                Console.WriteLine($"Added X-Correlation-ID: {correlationId}");

                // Send the message using SMTP client.
                try
                {
                    using (SmtpClient client = new SmtpClient(host, port, username, password))
                    {
                        client.Send(mailMessage);
                        Console.WriteLine("Email sent successfully.");
                    }
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
