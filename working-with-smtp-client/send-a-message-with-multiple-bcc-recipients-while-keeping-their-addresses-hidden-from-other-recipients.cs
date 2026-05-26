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

            // Guard against placeholder credentials to avoid live network calls during CI
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP settings detected. Skipping send operation.");
                return;
            }

            // Create and configure the SMTP client
            using (SmtpClient client = new SmtpClient(host, port, username, password))
            {
                // Build the email message
                MailMessage message = new MailMessage();
                message.From = new MailAddress("sender@example.com");
                message.To.Add(new MailAddress("recipient1@example.com"));
                message.Subject = "Test Email with BCC";
                message.Body = "This email has multiple BCC recipients.";

                // Add multiple BCC recipients (they will be hidden from other recipients)
                message.Bcc.Add(new MailAddress("bcc1@example.com"));
                message.Bcc.Add(new MailAddress("bcc2@example.com"));
                message.Bcc.Add(new MailAddress("bcc3@example.com"));

                // Send the message
                try
                {
                    client.Send(message);
                    Console.WriteLine("Message sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to send message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
