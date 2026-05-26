using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.AntiSpam;

namespace AsposeEmailSpamHeaderExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Placeholder SMTP server configuration
                string host = "smtp.example.com";
                int port = 587;
                string username = "username";
                string password = "password";

                // Guard against placeholder credentials/host to avoid real network calls
                if (host.Contains("example.com") || username == "username" || password == "password")
                {
                    Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping send operation.");
                    return;
                }

                // Create the email message
                MailMessage message = new MailMessage(
                    "sender@example.com",
                    "recipient@example.com",
                    "Test Subject",
                    "This is a test email body."
                );

                // Analyze the message for spam probability
                SpamAnalyzer analyzer = new SpamAnalyzer();
                double spamScore = analyzer.Test(message);

                // Add custom X-Spam-Score header
                message.Headers.Add("X-Spam-Score", spamScore.ToString("F2"));

                // Send the message using SmtpClient
                using (SmtpClient client = new SmtpClient(host, port, SecurityOptions.Auto))
                {
                    client.Username = username;
                    client.Password = password;

                    try
                    {
                        client.Send(message);
                        Console.WriteLine("Message sent successfully with X-Spam-Score header.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error sending message: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
