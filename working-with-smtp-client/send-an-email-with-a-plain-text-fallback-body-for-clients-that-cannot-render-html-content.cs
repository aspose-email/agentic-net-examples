using System;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // SMTP server configuration (placeholder values)
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls
            if (host.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP host detected. Skipping send operation.");
                return;
            }

            // Create and configure the SMTP client
            using (SmtpClient client = new SmtpClient(host, port, username, password))
            {
                try
                {
                    client.SecurityOptions = SecurityOptions.Auto;

                    // Build the email message with HTML body and plain‑text fallback
                    MailMessage message = new MailMessage();
                    message.From = "sender@example.com";
                    message.To.Add("recipient@example.com");
                    message.Subject = "Test email with HTML and plain text fallback";

                    // Plain‑text fallback
                    message.Body = "This is the plain text fallback body.";

                    // HTML body
                    message.IsBodyHtml = true;
                    message.HtmlBody = "<html><body><h1>Hello</h1><p>This is an <b>HTML</b> email.</p></body></html>";

                    // Send the message
                    client.Send(message);
                    Console.WriteLine("Email sent successfully.");
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
