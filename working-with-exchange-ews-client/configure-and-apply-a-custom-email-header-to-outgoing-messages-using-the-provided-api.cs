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
            string username = "username";
            string password = "password";

            // Guard: skip sending when placeholders are used
            if (host.Contains("example.com") || username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder SMTP credentials detected. Skipping send operation.");
                return;
            }

            // Create and configure the SMTP client
            using (SmtpClient client = new SmtpClient(host, username, password))
            {
                try
                {
                    // Build the email message
                    using (MailMessage message = new MailMessage())
                    {
                        message.From = new MailAddress("sender@example.com");
                        message.To.Add(new MailAddress("recipient@example.com"));
                        message.Subject = "Test Email with Custom Header";
                        message.Body = "This email contains a custom header.";

                        // Apply a custom header
                        message.Headers.Add("X-Custom-Header", "MyHeaderValue");

                        // Send the message
                        client.Send(message);
                        Console.WriteLine("Message sent successfully.");
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
