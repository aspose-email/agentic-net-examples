using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Configuration
            string smtpHost = "smtp.example.com";
            int smtpPort = 587;
            string smtpUser = "user@example.com";
            string smtpPass = "password";
            int timeoutMilliseconds = 10000; // 10 seconds

            // Skip execution when placeholder credentials are detected
            if (smtpHost.Contains("example.com") || string.IsNullOrWhiteSpace(smtpUser) || string.IsNullOrWhiteSpace(smtpPass))
            {
                Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping send operation.");
                return;
            }

            // Create a simple email message
            MailMessage message = new MailMessage();
            message.From = smtpUser;
            message.To.Add(smtpUser);
            message.Subject = "Test Email";
            message.Body = "This is a test email sent using Aspose.Email.";

            // Initialize the SMTP client with timeout watchdog
            using (SmtpClient client = new SmtpClient(smtpHost, smtpPort, smtpUser, smtpPass, SecurityOptions.Auto))
            {
                try
                {
                    client.Timeout = timeoutMilliseconds; // watchdog timeout in milliseconds
                    client.Send(message);
                    Console.WriteLine("Email sent successfully.");
                }
                catch (Exception ex)
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
