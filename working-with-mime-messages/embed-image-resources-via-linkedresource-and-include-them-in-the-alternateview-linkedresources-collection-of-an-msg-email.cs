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
            string imagePath = "1.jpg";
            string outputPath = "EmbeddedImage_out.msg";

            // Ensure the image file exists; create a minimal placeholder if missing
            if (!File.Exists(imagePath))
            {
                using (FileStream fs = File.Create(imagePath))
                {
                    // Minimal JPEG header (optional)
                    byte[] jpegHeader = new byte[] { 0xFF, 0xD8, 0xFF, 0xD9 };
                    fs.Write(jpegHeader, 0, jpegHeader.Length);
                }
            }

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create the email message
            using (MailMessage message = new MailMessage())
            {
                message.From = "AndrewIrwin@from.com";
                message.To = "SusanMarc@to.com";
                message.Subject = "This is an email";

                // Create the plain text view
                using (AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                    "This is my plain text content", null, "text/plain"))
                {
                    // Create the HTML view with a CID reference to the image
                    using (AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                        "Here is an embedded image. <img src=cid:barcode>", null, "text/html"))
                    {
                        // Create the linked resource (embedded image)
                        using (LinkedResource barcode = new LinkedResource(imagePath, MediaTypeNames.Image.Jpeg))
                        {
                            barcode.ContentId = "barcode";

                            // Add the linked resource to the message
                            message.LinkedResources.Add(barcode);

                            // Add alternate views to the message
                            message.AlternateViews.Add(plainView);
                            message.AlternateViews.Add(htmlView);

                            // Save the message as an MSG file with Unicode options
                            message.Save(outputPath, SaveOptions.DefaultMsgUnicode);
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
