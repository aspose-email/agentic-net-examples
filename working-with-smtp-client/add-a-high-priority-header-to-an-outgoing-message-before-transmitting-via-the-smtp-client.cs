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
            // Placeholder SMTP server details
            string smtpHost = "smtp.example.com";
            int smtpPort = 587;
            string smtpUser = "username";
            string smtpPass = "password";

            // Guard against placeholder credentials/host
            if (smtpHost.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP host detected. Skipping send operation.");
                return;
            }

            // Create the email message
            MailMessage message = new MailMessage(
                "sender@example.com",
                "recipient@example.com",
                "Test Subject",
                "This is the body of the email."
            );

            // Add high‑priority header
            message.Headers.Add("X-Priority", "1 (Highest)");
            message.Headers.Add("Priority", "Urgent");

            // Send the message via SMTP
            using (SmtpClient client = new SmtpClient(smtpHost, smtpPort))
            {
                client.Username = smtpUser;
                client.Password = smtpPass;
                client.SecurityOptions = SecurityOptions.Auto;

                try
                {
                    client.Send(message);
                    Console.WriteLine("Message sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"SMTP send failed: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
