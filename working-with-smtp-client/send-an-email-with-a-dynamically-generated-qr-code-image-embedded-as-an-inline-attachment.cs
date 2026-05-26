using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder SMTP settings – replace with real values.
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials to avoid live network calls.
            if (host.Contains("example.com") || username.Contains("example.com") || password == "password")
            {
                Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping send operation.");
                return;
            }

            // Placeholder PNG image (1x1 pixel) representing a QR code.
            const string base64Png = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8/x8AAwMCAO+XK6cAAAAASUVORK5CYII=";
            byte[] qrImageBytes = Convert.FromBase64String(base64Png);

            using (MemoryStream imageStream = new MemoryStream(qrImageBytes))
            {
                // Build the email message.
                using (MailMessage message = new MailMessage())
                {
                    message.From = "sender@example.com";
                    message.To.Add("recipient@example.com");
                    message.Subject = "Email with QR Code";
                    message.Body = "Please find the QR code attached inline.";

                    // Create an inline attachment from the QR image.
                    Attachment inlineAttachment = new Attachment(imageStream, "qr.png", "image/png")
                    {
                        ContentId = "qrCodeImage"
                    };
                    // Mark the attachment as inline.
                    inlineAttachment.ContentDisposition.Inline = true;

                    message.Attachments.Add(inlineAttachment);

                    // Reference the inline image in the HTML body (optional).
                    message.IsBodyHtml = true;
                    message.HtmlBody = "<html><body><p>Please find the QR code below:</p><img src=\"cid:qrCodeImage\" /></body></html>";

                    // Send the email using SMTP client.
                    using (SmtpClient client = new SmtpClient(host, port, username, password))
                    {
                        try
                        {
                            client.Send(message);
                            Console.WriteLine("Email sent successfully.");
                        }
                        catch (AsposeException ex)
                        {
                            Console.Error.WriteLine($"Failed to send email: {ex.Message}");
                        }
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
