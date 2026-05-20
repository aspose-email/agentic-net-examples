using System;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Clients;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string host = "smtp.example.com";
            int port = 465;
            string username = "user@example.com";
            string password = "password";

            // Skip real network call when placeholder values are used
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping send operation.");
                return;
            }

            using (SmtpClient client = new SmtpClient(host, port, username, password, SecurityOptions.SSLImplicit))
            {
                try
                {
                    using (MailMessage message = new MailMessage())
                    {
                        message.From = new MailAddress(username);
                        message.To.Add("recipient@example.com");
                        message.Subject = "Test Email";
                        message.Body = "This is a plain-text test email sent via Aspose.Email SMTP client.";

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
