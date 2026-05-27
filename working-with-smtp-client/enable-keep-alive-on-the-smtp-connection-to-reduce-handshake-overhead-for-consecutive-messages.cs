using System;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp.Models;

class Program
{
    static void Main()
    {
        try
        {
            // SMTP server configuration (placeholder values)
            string host = "smtp.example.com";
            string username = "user@example.com";
            string password = "password";

            // Skip actual network call when placeholders are used
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping send operation.");
                return;
            }

            // Create and configure the SMTP client
            using (SmtpClient client = new SmtpClient(host, username, password))
            {
                // Enable keep‑alive by creating a reusable connection
                using (IConnection connection = client.CreateConnection())
                {
                    // First message
                    MailMessage message1 = new MailMessage();
                    message1.From = username;
                    message1.To.Add("recipient1@example.com");
                    message1.Subject = "First Message";
                    message1.Body = "Hello from the first message.";

                    // Second message
                    MailMessage message2 = new MailMessage();
                    message2.From = username;
                    message2.To.Add("recipient2@example.com");
                    message2.Subject = "Second Message";
                    message2.Body = "Hello from the second message.";

                    // Send both messages using the same connection (keep‑alive)
                    client.Send(connection, message1);
                    client.Send(connection, message2);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
