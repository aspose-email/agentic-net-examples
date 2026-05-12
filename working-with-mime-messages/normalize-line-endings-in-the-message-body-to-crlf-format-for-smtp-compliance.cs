using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Create a mail message with mixed line endings in the body
            MailMessage message = new MailMessage();
            message.From = "sender@example.com";
            message.To.Add("recipient@example.com");
            message.Subject = "Test Email";

            string mixedBody = "Line1\rLine2\nLine3\r\nLine4";
            // Normalize line endings to CRLF (\r\n)
            string[] lines = mixedBody.Split(new[] { "\r\n", "\n", "\r" }, StringSplitOptions.None);
            string normalizedBody = string.Join("\r\n", lines);
            message.Body = normalizedBody;

            // Placeholder SMTP configuration
            string smtpHost = "smtp.example.com";
            int smtpPort = 25;
            string username = "user";
            string password = "pass";

            // Skip sending when placeholder credentials are detected
            if (smtpHost.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP settings detected. Skipping send operation.");
                return;
            }

            using (SmtpClient client = new SmtpClient(smtpHost, smtpPort))
            {
                try
                {
                    client.Username = username;
                    client.Password = password;
                    client.Send(message);
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
