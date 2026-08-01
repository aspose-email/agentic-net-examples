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
            // Input image path
            string imagePath = "1.jpg";

            // Verify the image file exists
            if (!File.Exists(imagePath))
            {
                Console.Error.WriteLine($"Image file not found: {imagePath}");
                return;
            }

            // Output MSG file path
            string outputMsgPath = "EmbeddedImage_out.msg";

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputMsgPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create the email message
            MailMessage eml = new MailMessage
            {
                From = "AndrewIrwin@from.com",
                To = "SusanMarc@to.com",
                Subject = "This is an email"
            };

            // Plain text view (fallback for non‑HTML clients)
            AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                "This is my plain text content",
                null,
                "text/plain");

            // HTML view with a CID reference to the embedded image
            AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                "Here is an embedded image. <img src=cid:barcode>",
                null,
                "text/html");

            // Create the linked resource (embedded image) and assign a Content‑Id
            LinkedResource barcode = new LinkedResource(imagePath, MediaTypeNames.Image.Jpeg)
            {
                ContentId = "barcode"
            };

            // Attach the linked resource and alternate views to the message
            eml.LinkedResources.Add(barcode);
            eml.AlternateViews.Add(plainView);
            eml.AlternateViews.Add(htmlView);

            // Save the message as a MSG file with Unicode support
            eml.Save(outputMsgPath, SaveOptions.DefaultMsgUnicode);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
