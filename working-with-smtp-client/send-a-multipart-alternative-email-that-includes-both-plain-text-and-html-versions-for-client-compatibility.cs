using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder SMTP credentials – replace with real values.
            string smtpHost = "smtp.example.com";
            int smtpPort = 587;
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder values to avoid real network calls during CI.
            if (smtpHost.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping send operation.");
                return;
            }

            // Create the SMTP client.
            using (SmtpClient client = new SmtpClient(smtpHost, smtpPort, username, password))
            {
                client.SecurityOptions = SecurityOptions.Auto;

                // Create a mail message.
                MailMessage message = new MailMessage
                {
                    From = "sender@example.com",
                    Subject = "Multipart/Alternative Email"
                };
                message.To.Add("recipient@example.com");

                // Plain‑text part.
                AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                    "This is the plain‑text version of the email.",
                    new ContentType("text/plain"));

                // HTML part.
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                    "<html><body><h1>This is the HTML version of the email.</h1></body></html>",
                    new ContentType("text/html"));

                // Add both views to the message.
                message.AlternateViews.Add(plainView);
                message.AlternateViews.Add(htmlView);

                // Send the message.
                client.Send(message);
                Console.WriteLine("Email sent successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
