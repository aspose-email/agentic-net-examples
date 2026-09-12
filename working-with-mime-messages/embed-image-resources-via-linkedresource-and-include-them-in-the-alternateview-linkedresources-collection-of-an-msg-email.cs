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
            // Paths for the image and output MSG file
            string imagePath = "1.jpg";
            string outputPath = "EmbeddedImage_out.msg";

            // Ensure the image file exists; create a minimal placeholder if missing
            if (!File.Exists(imagePath))
            {
                // Minimal JPEG header (SOI and EOI) to form a valid image file
                byte[] jpegPlaceholder = new byte[] { 0xFF, 0xD8, 0xFF, 0xD9 };
                File.WriteAllBytes(imagePath, jpegPlaceholder);
            }

            // Create the email message
            MailMessage eml = new MailMessage();
            eml.From = "AndrewIrwin@from.com";
            eml.To = "SusanMarc@to.com";
            eml.Subject = "This is an email";

            // Plain‑text view (for clients that do not support HTML)
            AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                "This is my plain text content", null, "text/plain");

            // HTML view with an embedded image referenced by Content‑Id "barcode"
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                "Here is an embedded image. <img src=cid:barcode>", null, "text/html");

            // Linked resource representing the embedded image
            LinkedResource barcode = new LinkedResource(imagePath, MediaTypeNames.Image.Jpeg)
            {
                ContentId = "barcode"
            };

            // Attach the linked resource and alternate views to the message
            eml.LinkedResources.Add(barcode);
            eml.AlternateViews.Add(plainView);
            eml.AlternateViews.Add(htmlView);

            // Save the message as an MSG file with Unicode support
            eml.Save(outputPath, SaveOptions.DefaultMsgUnicode);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
