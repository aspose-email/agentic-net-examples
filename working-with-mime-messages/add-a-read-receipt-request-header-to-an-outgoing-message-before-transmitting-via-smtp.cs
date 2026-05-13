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
            // Placeholder SMTP configuration
            string smtpHost = "smtp.example.com";
            int smtpPort = 587;
            string smtpUsername = "user@example.com";
            string smtpPassword = "password";

            // Guard against placeholder credentials/host
            if (smtpHost.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping send operation.");
                return;
            }

            // Create the email message
            using (MailMessage message = new MailMessage("sender@example.com", "recipient@example.com", "Test Subject", "Hello world"))
            {
                // Request a read receipt
                message.ReadReceiptTo = "sender@example.com";

                // Send the message via SMTP
                try
                {
                    using (SmtpClient client = new SmtpClient(smtpHost, smtpPort))
                    {
                        client.Username = smtpUsername;
                        client.Password = smtpPassword;
                        client.SecurityOptions = SecurityOptions.Auto;
                        client.Send(message);
                        Console.WriteLine("Message sent successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"SMTP send failed: {ex.Message}");
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
