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
            // Placeholder SMTP configuration (replace with real values for actual use)
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid live network calls during CI
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping send operation.");
                return;
            }

            // Initialize the SMTP client
            using (SmtpClient client = new SmtpClient(host, port, username, password))
            {
                // Optional: configure security options, timeout, etc.
                client.SecurityOptions = SecurityOptions.Auto;

                // Create a mail message
                using (MailMessage message = new MailMessage())
                {
                    message.From = new MailAddress("sender@example.com");
                    message.To.Add("recipient@example.com");
                    message.Subject = "Test message with List-Id header";
                    message.Body = "This is a test email.";

                    // Add List-Id header for mailing list identification
                    message.Headers.Add("List-Id", "<mylist.example.com>");

                    // Send the message
                    client.Send(message);
                    Console.WriteLine("Message sent successfully.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
