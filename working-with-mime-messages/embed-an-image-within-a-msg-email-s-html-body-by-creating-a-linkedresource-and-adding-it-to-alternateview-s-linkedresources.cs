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
            string outputPath = "EmbeddedImage_out.msg";

            // Verify image file exists
            if (!File.Exists(imagePath))
            {
                Console.Error.WriteLine($"Image file not found: {imagePath}");
                return;
            }

            // Ensure output directory exists
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
                using (AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                    "This is my plain text content", null, "text/plain"))
                // HTML view with cid reference
                using (AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                    "Here is an embedded image. <img src=cid:barcode>", null, "text/html"))
                // Linked resource for the image
                using (LinkedResource linkedResource = new LinkedResource(imagePath, MediaTypeNames.Image.Jpeg))
                {
                    linkedResource.ContentId = "barcode";

                    // Attach the linked resource to the HTML view
                    htmlView.LinkedResources.Add(linkedResource);

                    // Add alternate views to the message
                    message.AlternateViews.Add(plainView);
                    message.AlternateViews.Add(htmlView);

                    // Save the message as MSG (Unicode)
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
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
