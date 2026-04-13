using System;
using System.IO;
using System.Net.Mime;
using Aspose.Email;
class Program
{
    static void Main()
    {
        try
        {
            string imagePath = "barcode.jpg";
            if (!File.Exists(imagePath))
            {
                Console.Error.WriteLine($"Image file '{imagePath}' not found.");
                return;
            }

            // Create the mail message
            MailMessage mailMessage = new MailMessage();
            mailMessage.From = "sender@example.com";
            mailMessage.To = "recipient@example.com";
            mailMessage.Subject = "HTML Email with Inline Image";

            // Plain text view
            AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                "This is the plain text version of the email.", null, "text/plain");

            // HTML view with content‑ID reference
            string htmlBody = "<html><body><h1>Hello</h1><img src=\"cid:barcode\" alt=\"Barcode\"/></body></html>";
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                htmlBody, null, "text/html");

            // Linked resource (inline image)
            LinkedResource linkedResource = new LinkedResource(imagePath, MediaTypeNames.Image.Jpeg);
            linkedResource.ContentId = "barcode";

            // Attach resources and views
            mailMessage.LinkedResources.Add(linkedResource);
            mailMessage.AlternateViews.Add(plainView);
            mailMessage.AlternateViews.Add(htmlView);

            // Save the message
            string outputPath = "HtmlEmailWithImage.msg";
            try
            {
                mailMessage.Save(outputPath, SaveOptions.DefaultMsgUnicode);
                Console.WriteLine($"Message saved to '{outputPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to save message: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
