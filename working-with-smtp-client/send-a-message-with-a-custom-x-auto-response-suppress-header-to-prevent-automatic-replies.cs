using System;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // SMTP server configuration (replace with real values)
            string host = "smtp.example.com";
            int port = 587;
            string username = "username";
            string password = "password";

            // Guard against placeholder credentials to avoid external calls during CI
            if (host.Contains("example.com") || username.Equals("username", StringComparison.OrdinalIgnoreCase))
            {
                Console.Error.WriteLine("Placeholder SMTP settings detected. Skipping send operation.");
                return;
            }

            // Create the mail message
            MailMessage message = new MailMessage(
                "from@example.com",
                "to@example.com",
                "Test Subject",
                "This is a test email body."
            );

            // Add custom header to suppress automatic replies
            message.Headers.Add("X-Auto-Response-Suppress", "All");

            // Send the message using SmtpClient
            using (SmtpClient client = new SmtpClient(host, port, username, password))
            {
                try
                {
                    client.Send(message);
                    Console.WriteLine("Message sent successfully.");
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
