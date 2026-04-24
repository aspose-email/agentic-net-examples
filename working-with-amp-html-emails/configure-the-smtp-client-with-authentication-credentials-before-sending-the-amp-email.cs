using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Amp;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Clients.Smtp.Models;

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
            if (host.Contains("example.com") || username == "username")
            {
                Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping send.");
                return;
            }

            // Create an AMP email message
            AmpMessage ampMessage = new AmpMessage();
            ampMessage.From = new MailAddress("sender@example.com", "Sender");
            ampMessage.To.Add(new MailAddress("recipient@example.com", "Recipient"));
            ampMessage.Subject = "AMP Email Example";
            ampMessage.Body = "This is the plain‑text fallback.";
            ampMessage.IsBodyHtml = true;
            ampMessage.HtmlBody = "<html><body><h1>Hello AMP</h1></body></html>";
            // Optional: set AMP specific HTML body
            // ampMessage.AmpHtmlBody = "<amp-email>...</amp-email>";

            // Configure the SMTP client with authentication credentials
            using (SmtpClient client = new SmtpClient(host, port, username, password, SecurityOptions.Auto))
            {
                try
                {
                    client.Send(ampMessage);
                    Console.WriteLine("AMP email sent successfully.");
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
