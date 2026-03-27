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
            // Verify the image file exists
            string imagePath = "1.jpg";
            if (!File.Exists(imagePath))
            {
                Console.Error.WriteLine($"Image file not found: {imagePath}");
                return;
            }

            // Ensure output directory exists
            string outputPath = "EmbeddedImage_out.msg";
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create the email message
            using (MailMessage eml = new MailMessage())
            {
                eml.From = "AndrewIrwin@from.com";
                eml.To.Add("SusanMarc@to.com");
                eml.Subject = "This is an email";

                // Plain text view
                using (AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                    "This is my plain text content", null, "text/plain"))
                {
                    // HTML view with embedded image reference
                    using (AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                        "Here is an embedded image. <img src=cid:barcode>", null, "text/html"))
                    {
                        // Linked resource for the image
                        using (LinkedResource barcode = new LinkedResource(
                            imagePath, Aspose.Email.Mime.MediaTypeNames.Image.Jpeg))
                        {
                            barcode.ContentId = "barcode";

                            // Attach resources and views to the message
                            eml.LinkedResources.Add(barcode);
                            eml.AlternateViews.Add(plainView);
                            eml.AlternateViews.Add(htmlView);

                            // Save the message as MSG
                            eml.Save(outputPath, SaveOptions.DefaultMsgUnicode);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
