using System;
using System.IO;
using System.Text;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Google;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder Gmail credentials
            string clientId = "YOUR_CLIENT_ID";
            string clientSecret = "YOUR_CLIENT_SECRET";
            string refreshToken = "YOUR_REFRESH_TOKEN";

            // Skip execution if placeholder credentials are present
            if (clientId.StartsWith("YOUR_") || clientSecret.StartsWith("YOUR_") || refreshToken.StartsWith("YOUR_"))
            {
                Console.Error.WriteLine("Placeholder Gmail credentials detected. Skipping send operation.");
                return;
            }

            // Create Gmail client (proxy parameter set to null)
            IGmailClient gmailClient = GmailClient.GetInstance(clientId, null, clientSecret, refreshToken);

            // Prepare attachment file
            string attachmentPath = "attachment.txt";
            if (!File.Exists(attachmentPath))
            {
                try
                {
                    File.WriteAllText(attachmentPath, "Sample attachment content.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create attachment file: {ex.Message}");
                    return;
                }
            }

            // Build the email message with text, HTML, and attachment
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Test email with multipart/mixed";
                message.Body = "This is the plain text body.";
                message.IsBodyHtml = false;

                // HTML alternate view
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                    "<html><body><h1>Hello</h1><p>This is HTML body.</p></body></html>",
                    Encoding.UTF8,
                    "text/html");
                message.AlternateViews.Add(htmlView);

                // Attachment
                Attachment attachment = new Attachment(attachmentPath);
                message.Attachments.Add(attachment);

                // Send the message
                try
                {
                    string sentId = gmailClient.SendMessage(message);
                    Console.WriteLine($"Message sent successfully. Id: {sentId}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to send email: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
