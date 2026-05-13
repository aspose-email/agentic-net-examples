using System;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder SMTP server details
            string smtpHost = "smtp.example.com";
            int smtpPort = 587;
            string smtpUser = "user@example.com";
            string smtpPassword = "password";

            // Skip actual sending when placeholders are detected
            if (smtpHost.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP host detected. Skipping send operation.");
                return;
            }

            // Create and configure the SMTP client
            using (SmtpClient client = new SmtpClient(smtpHost, smtpPort, smtpUser, smtpPassword))
            {
                try
                {
                    client.ValidateCredentials();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"SMTP connection/validation failed: {ex.Message}");
                    return;
                }

                // Compose the email message
                using (MailMessage message = new MailMessage())
                {
                    message.From = "sender@example.com";
                    message.To.Add("recipient@example.com");
                    message.Subject = "Test Email";
                    message.Body = "This is a test email sent via Aspose.Email.";

                    // Remove the X-Spam-Status header if it exists
                    try
                    {
                        message.Headers.Remove("X-Spam-Status");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to remove header: {ex.Message}");
                    }

                    // Send the message
                    try
                    {
                        client.Send(message);
                        Console.WriteLine("Message sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Sending failed: {ex.Message}");
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
