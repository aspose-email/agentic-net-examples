using Aspose.Email.Clients;
using System;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            string smtpHost = "smtp.example.com";
            int smtpPort = 587;
            string smtpUsername = "user@example.com";
            string smtpPassword = "password";

            // Skip execution when placeholder credentials are detected
            if (smtpHost.Contains("example.com") ||
                smtpUsername.Contains("example.com") ||
                string.IsNullOrWhiteSpace(smtpPassword))
            {
                Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping email send.");
                return;
            }

            using (SmtpClient client = new SmtpClient(smtpHost, smtpPort, smtpUsername, smtpPassword, SecurityOptions.Auto))
            {
                // Bind to a specific local network interface
                client.BindIPEndPoint += remoteEndPoint => new IPEndPoint(IPAddress.Parse("192.168.1.100"), 0);

                try
                {
                    client.ValidateCredentials();

                    using (MailMessage message = new MailMessage())
                    {
                        message.From = new MailAddress(smtpUsername);
                        message.To.Add(new MailAddress("recipient@example.com"));
                        message.Subject = "Test email from specific interface";
                        message.Body = "This email was sent using a specific local network interface.";

                        client.Send(message);
                        Console.WriteLine("Email sent successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during email operation: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
