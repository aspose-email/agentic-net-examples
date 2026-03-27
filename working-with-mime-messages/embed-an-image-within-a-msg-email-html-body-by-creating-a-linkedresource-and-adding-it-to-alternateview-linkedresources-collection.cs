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
            // Define file paths
            string imagePath = "barcode.jpg";
            string outputPath = "EmbeddedImage_out.msg";

            // Ensure the image file exists; create a minimal placeholder if missing
            if (!File.Exists(imagePath))
            {
                try
                {
                    // Create a tiny placeholder file (empty content)
                    File.WriteAllBytes(imagePath, new byte[0]);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder image: {ex.Message}");
                    return;
                }
            }

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Create the email message
            using (MailMessage message = new MailMessage())
            {
                message.From = "AndrewIrwin@from.com";
                message.To = "SusanMarc@to.com";
                message.Subject = "This is an email";

                // Plain text view
                AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                    "This is my plain text content", null, "text/plain");

                // HTML view with embedded image reference
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                    "Here is an embedded image. <img src=cid:barcode>", null, "text/html");

                // Create the linked resource (image) and assign ContentId
                LinkedResource barcode = new LinkedResource(imagePath, MediaTypeNames.Image.Jpeg)
                {
                    ContentId = "barcode"
                };

                // Add linked resource to the message
                message.LinkedResources.Add(barcode);

                // Add alternate views to the message
                message.AlternateViews.Add(plainView);
                message.AlternateViews.Add(htmlView);

                // Save the message as MSG
                try
                {
                    message.Save(outputPath, SaveOptions.DefaultMsgUnicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
