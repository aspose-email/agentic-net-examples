using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mime;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string imagePath = "1.jpg";
            string outputPath = "EmbeddedImage_out.msg";

            // Verify the image file exists
            if (!File.Exists(imagePath))
            {
                Console.Error.WriteLine($"Image file '{imagePath}' not found.");
                return;
            }

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            using (MailMessage eml = new MailMessage())
            {
                eml.From = "AndrewIrwin@from.com";
                eml.To = "SusanMarc@to.com";
                eml.Subject = "This is an email";

                // Plain text view
                AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                    "This is my plain text content", null, "text/plain");

                // HTML view with CID reference
                string htmlContent = "Here is an embedded image. <img src=cid:barcode>";
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                    htmlContent, null, "text/html");

                // Linked resource for the image
                LinkedResource barcode = new LinkedResource(imagePath, Aspose.Email.Mime.MediaTypeNames.Image.Jpeg)
                {
                    ContentId = "barcode"
                };

                // Attach resources and views
                eml.LinkedResources.Add(barcode);
                eml.AlternateViews.Add(plainView);
                eml.AlternateViews.Add(htmlView);

                // Save as MSG
                eml.Save(outputPath, SaveOptions.DefaultMsgUnicode);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
