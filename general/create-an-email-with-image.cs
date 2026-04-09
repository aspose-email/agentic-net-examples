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
            // Define paths
            string imagePath = "barcode.jpg";
            string outputPath = "EmbeddedImage_out.msg";

            // Ensure the image file exists; create a minimal placeholder if missing
            try
            {
                if (!File.Exists(imagePath))
                {
                    File.WriteAllBytes(imagePath, new byte[0]);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare image file: {ex.Message}");
                return;
            }

            // Ensure the output directory exists
            try
            {
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Create the email message
            using (MailMessage eml = new MailMessage())
            {
                eml.From = "AndrewIrwin@from.com";
                eml.To.Add("SusanMarc@to.com");
                eml.Subject = "This is an email";

                // Plain text view
                AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                    "This is my plain text content", null, "text/plain");

                // HTML view with embedded image reference (cid)
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                    "Here is an embedded image. <img src=cid:barcode>", null, "text/html");

                // Linked resource for the embedded image
                LinkedResource barcode = new LinkedResource(imagePath, MediaTypeNames.Image.Jpeg)
                {
                    ContentId = "barcode"
                };

                // Attach the linked resource to the HTML view
                htmlView.LinkedResources.Add(barcode);

                // Add alternate views to the message
                eml.AlternateViews.Add(plainView);
                eml.AlternateViews.Add(htmlView);

                // Save the message to a MSG file
                try
                {
                    eml.Save(outputPath, SaveOptions.DefaultMsgUnicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save email: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
