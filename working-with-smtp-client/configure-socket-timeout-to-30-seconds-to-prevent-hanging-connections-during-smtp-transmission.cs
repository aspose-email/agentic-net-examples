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
            string username = "user@example.com";
            string password = "password";

            // Skip execution when placeholder credentials are detected
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder SMTP settings detected. Skipping actual send.");
                return;
            }

            // Initialize the SMTP client
            using (SmtpClient client = new SmtpClient(host, port, username, password))
            {
                // Configure socket timeout to 30 seconds (30000 ms)
                client.Timeout = 30000;

                // Create a simple email message
                using (MailMessage message = new MailMessage(
                    "from@example.com",
                    "to@example.com",
                    "Test Subject",
                    "This is a test email."))
                {
                    try
                    {
                        client.Send(message);
                        Console.WriteLine("Email sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to send email: {ex.Message}");
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
