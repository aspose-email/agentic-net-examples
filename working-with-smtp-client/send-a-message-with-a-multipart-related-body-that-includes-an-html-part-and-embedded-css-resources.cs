using Aspose.Email.Clients;
using System;
using Aspose.Email;
using Aspose.Email.Mime;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Create the email message
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Test email with embedded CSS";

                // Plain‑text view
                AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                    "This is the plain text body.", null, "text/plain");

                // HTML view that references the embedded CSS via CID
                string htmlBody = "<html><head><link rel=\"stylesheet\" type=\"text/css\" href=\"cid:styles.css\"/></head>"
                                + "<body><h1>Hello</h1><p>This is HTML body.</p></body></html>";
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                    htmlBody, null, "text/html");

                // Embedded CSS as a linked resource
                string cssContent = "h1 { color: blue; } p { font-size: 14px; }";
                ContentType cssContentType = new ContentType("text/css");
                LinkedResource cssResource = LinkedResource.CreateLinkedResourceFromString(
                    cssContent, cssContentType);
                cssResource.ContentId = "styles.css";

                // Attach resources and views to the message
                message.LinkedResources.Add(cssResource);
                message.AlternateViews.Add(plainView);
                message.AlternateViews.Add(htmlView);

                // Placeholder SMTP credentials – replace with real values to enable sending
                string smtpHost = "";
                int smtpPort = 587;
                string username = "";
                string password = "";

                if (string.IsNullOrWhiteSpace(smtpHost) ||
                    string.IsNullOrWhiteSpace(username) ||
                    string.IsNullOrWhiteSpace(password))
                {
                    Console.Error.WriteLine("Placeholder SMTP credentials are not set. Skipping send operation.");
                    return;
                }

                // Send the message using SmtpClient
                using (SmtpClient client = new SmtpClient(smtpHost, smtpPort, username, password))
                {
                    client.SecurityOptions = SecurityOptions.Auto;
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
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
