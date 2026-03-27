using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mime;

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
                    File.WriteAllBytes(imagePath, new byte[] { 0xFF, 0xD8, 0xFF, 0xE0, 0x00, 0x10, 0x4A, 0x46, 0x49, 0x46, 0x00, 0x01 });
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder image: {ex.Message}");
                    return;
                }
            }

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
            if (!Directory.Exists(outputDir))
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
                message.To.Add("SusanMarc@to.com");
                message.Subject = "This is an email";

                // Create plain text view
                AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                    "This is my plain text content", null, "text/plain");

                // Create HTML view with a placeholder for the embedded image
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                    "Here is an embedded image. <img src=cid:barcode>", null, "text/html");

                // Add the views to the message
                message.AlternateViews.Add(plainView);
                message.AlternateViews.Add(htmlView);

                // Add linked resource to the first alternate view (plainView)
                // Use fully qualified Aspose.Email.Mime.MediaTypeNames to avoid ambiguity
                LinkedResource linkedResource = new LinkedResource(imagePath, Aspose.Email.Mime.MediaTypeNames.Image.Jpeg)
                {
                    ContentId = "barcode"
                };
                message.AlternateViews[0].LinkedResources.Add(linkedResource);

                // Save the message to MSG format
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
