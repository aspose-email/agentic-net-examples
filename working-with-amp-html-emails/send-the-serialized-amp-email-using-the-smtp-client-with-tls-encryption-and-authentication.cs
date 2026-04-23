using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Amp;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Define SMTP server settings (replace with real values)
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid real network calls in CI
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP settings detected. Skipping send operation.");
                return;
            }

            // Create an AMP email message
            using (AmpMessage ampMessage = new AmpMessage())
            {
                ampMessage.From = new MailAddress("sender@example.com", "Sender Name");
                ampMessage.To.Add(new MailAddress("recipient@example.com", "Recipient Name"));
                ampMessage.Subject = "AMP Email Example";
                ampMessage.Body = "This is the plain text fallback.";
                ampMessage.IsBodyHtml = true;
                ampMessage.HtmlBody = "<p>This is the HTML fallback.</p>";
                ampMessage.AmpHtmlBody = @"
                    <amp-email>
                        <head>
                            <style amp4email-boilerplate>body{visibility:hidden}</style>
                            <script async src=""https://cdn.ampproject.org/v0.js""></script>
                        </head>
                        <body>
                            <h1>Hello from AMP Email!</h1>
                        </body>
                    </amp-email>";

                // Send the message using TLS (STARTTLS) and authentication
                try
                {
                    using (SmtpClient client = new SmtpClient(host, port, username, password, SecurityOptions.SSLExplicit))
                    {
                        client.Send(ampMessage);
                        Console.WriteLine("AMP email sent successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error sending email: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
