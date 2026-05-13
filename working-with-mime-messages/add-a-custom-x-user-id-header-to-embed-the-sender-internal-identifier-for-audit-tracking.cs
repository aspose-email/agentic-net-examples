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
            string smtpHost = "smtp.example.com";
            int smtpPort = 25;
            string smtpUsername = "user";
            string smtpPassword = "password";

            // Skip sending when placeholder values are detected
            if (smtpHost.Contains("example.com") || smtpUsername == "user" || smtpPassword == "password")
            {
                Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping send operation.");
                return;
            }

            // Create the email message
            using (MailMessage msg = new MailMessage())
            {
                msg.From = new MailAddress("sender@example.com");
                msg.To.Add(new MailAddress("recipient@example.com"));
                msg.Subject = "Test Email with Custom Header";
                msg.Body = "This email contains a custom X-User-ID header.";

                // Add custom audit header
                msg.Headers.Add("X-User-ID", "12345");

                // Send the message using SMTP client
                using (SmtpClient client = new SmtpClient(smtpHost, smtpPort, smtpUsername, smtpPassword))
                {
                    try
                    {
                        client.Send(msg);
                        Console.WriteLine("Email sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error sending email: {ex.Message}");
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
