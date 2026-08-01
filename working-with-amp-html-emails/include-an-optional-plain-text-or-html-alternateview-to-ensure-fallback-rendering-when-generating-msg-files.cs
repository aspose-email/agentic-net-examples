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
            // Prepare resources
            string imagePath = "1.jpg";
            if (!File.Exists(imagePath))
            {
                // Create an empty placeholder image file if missing
                File.WriteAllBytes(imagePath, new byte[0]);
            }

            string outputPath = "EmbeddedImage_out.msg";
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create the email message
            MailMessage message = new MailMessage
            {
                From = "AndrewIrwin@from.com",
                To = "SusanMarc@to.com",
                Subject = "This is an email"
            };

            // Plain‑text view (fallback for clients without HTML support)
            AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                "This is my plain text content",
                null,
                "text/plain");

            // HTML view with an embedded image reference (cid:barcode)
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                "Here is an embedded image. <img src=cid:barcode>",
                null,
                "text/html");

            // Linked resource for the embedded image
            LinkedResource barcode = new LinkedResource(imagePath, MediaTypeNames.Image.Jpeg)
            {
                ContentId = "barcode"
            };

            // Assemble the message
            message.LinkedResources.Add(barcode);
            message.AlternateViews.Add(plainView);
            message.AlternateViews.Add(htmlView);

            // Save as MSG with Unicode support
            message.Save(outputPath, SaveOptions.DefaultMsgUnicode);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            return;
        }
    }
}
