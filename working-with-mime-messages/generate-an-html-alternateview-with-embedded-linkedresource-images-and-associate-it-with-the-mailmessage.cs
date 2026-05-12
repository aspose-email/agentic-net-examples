using System;
using System.IO;
using System.Net.Mime;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Output message file
            string outputPath = "EmbeddedImage_out.msg";
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Image to embed
            string imagePath = "1.jpg";
            if (!File.Exists(imagePath))
            {
                // Create a minimal placeholder JPEG if missing
                try
                {
                    using (FileStream fs = File.Create(imagePath))
                    {
                        // JPEG SOI and EOI markers
                        byte[] jpegHeader = new byte[] { 0xFF, 0xD8, 0xFF, 0xD9 };
                        fs.Write(jpegHeader, 0, jpegHeader.Length);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder image: {ex.Message}");
                    return;
                }
            }

            using (MailMessage eml = new MailMessage())
            {
                eml.From = "AndrewIrwin@from.com";
                eml.To.Add("SusanMarc@to.com");
                eml.Subject = "This is an email";

                // Plain text alternate view
                AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                    "This is my plain text content", null, "text/plain");

                // HTML alternate view with CID reference
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                    "Here is an embedded image. <img src=cid:barcode>", null, "text/html");

                // Linked resource for the image
                using (LinkedResource barcode = new LinkedResource(imagePath, MediaTypeNames.Image.Jpeg))
                {
                    barcode.ContentId = "barcode";

                    // Associate linked resource with the HTML view
                    htmlView.LinkedResources.Add(barcode);

                    // Add alternate views to the message
                    eml.AlternateViews.Add(plainView);
                    eml.AlternateViews.Add(htmlView);

                    // Save the message
                    try
                    {
                        eml.Save(outputPath);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save message: {ex.Message}");
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
