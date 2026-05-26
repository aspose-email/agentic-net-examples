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
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP settings detected. Skipping send operation.");
                return;
            }

            // Create and configure the SMTP client
            using (SmtpClient client = new SmtpClient(host, port, username, password))
            {
                // Create a simple mail message
                MailMessage message = new MailMessage(
                    "from@example.com",
                    "to@example.com",
                    "Sample Subject",
                    "This is a test email body."
                );

                // Ensure a Date header (UTC) is present
                if (string.IsNullOrEmpty(message.Headers["Date"]))
                {
                    message.Headers["Date"] = DateTime.UtcNow.ToString("r");
                }

                // Send the message
                client.Send(message);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
