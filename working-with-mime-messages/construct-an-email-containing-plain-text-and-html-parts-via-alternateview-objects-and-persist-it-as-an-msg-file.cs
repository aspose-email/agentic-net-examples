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
            // Output MSG file path
            string outputPath = "EmbeddedImage_out.msg";

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Path to the image to embed
            string imagePath = "1.jpg";

            // Ensure the image file exists; create an empty placeholder if missing
            if (!File.Exists(imagePath))
            {
                File.WriteAllBytes(imagePath, new byte[0]);
            }

            // Create the email message
            using (MailMessage eml = new MailMessage())
            {
                eml.From = "AndrewIrwin@from.com";
                eml.To = "SusanMarc@to.com";
                eml.Subject = "This is an email";

                // Plain‑text view
                AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                    "This is my plain text content", null, "text/plain");

                // HTML view with a CID reference to the embedded image
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                    "Here is an embedded image. <img src=cid:barcode>", null, "text/html");

                // Linked resource for the embedded image
                LinkedResource barcode = new LinkedResource(imagePath, MediaTypeNames.Image.Jpeg)
                {
                    ContentId = "barcode"
                };

                // Attach resources and views to the message
                eml.LinkedResources.Add(barcode);
                eml.AlternateViews.Add(plainView);
                eml.AlternateViews.Add(htmlView);

                // Save the message as MSG with Unicode support
                eml.Save(outputPath, SaveOptions.DefaultMsgUnicode);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
