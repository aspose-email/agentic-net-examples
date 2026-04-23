using System;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            string smtpHost = "smtp.example.com";
            if (smtpHost.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP host detected. Skipping send.");
                return;
            }

            using (SmtpClient client = new SmtpClient(smtpHost, 25, "username", "password"))
            {
                try
                {
                    using (MailMessage message = new MailMessage())
                    {
                        message.From = new MailAddress("sender@example.com");
                        // Primary recipients
                        message.To.Add(new MailAddress("recipient1@example.com"));
                        message.To.Add(new MailAddress("recipient2@example.com"));
                        // CC recipients
                        message.CC.Add(new MailAddress("cc1@example.com"));
                        // BCC recipients
                        message.Bcc.Add(new MailAddress("bcc1@example.com"));
                        message.Subject = "Test Email";
                        message.Body = "This is a test email sent using Aspose.Email.";

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
