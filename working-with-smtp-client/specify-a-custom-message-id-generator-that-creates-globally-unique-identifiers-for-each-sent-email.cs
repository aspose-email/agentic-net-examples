using System;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Define SMTP connection parameters (placeholders)
            string smtpHost = "smtp.example.com";
            int smtpPort = 587;
            string smtpUsername = "user@example.com";
            string smtpPassword = "password";

            // Guard against placeholder credentials to avoid real network calls
            if (smtpHost.Contains("example.com") || smtpUsername.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping send operation.");
                return;
            }

            // Create a mail message
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = smtpUsername;
            mailMessage.To.Add("recipient@example.com");
            mailMessage.Subject = "Test Email with Custom Message-ID";
            mailMessage.Body = "This email uses a custom globally unique Message-ID.";

            // Assign a custom Message-ID using a GUID
            mailMessage.MessageId = GenerateMessageId();

            // Send the email using SmtpClient
            using (SmtpClient smtpClient = new SmtpClient(smtpHost, smtpPort, smtpUsername, smtpPassword))
            {
                try
                {
                    smtpClient.Send(mailMessage);
                    Console.WriteLine("Email sent successfully with Message-ID: " + mailMessage.MessageId);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to send email: " + ex.Message);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }

    // Generates a globally unique Message-ID in standard format
    private static string GenerateMessageId()
    {
        // Example format: <guid@customdomain.com>
        return $"<{Guid.NewGuid()}@customdomain.com>";
    }
}
