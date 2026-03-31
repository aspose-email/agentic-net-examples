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
            string imagePath = "1.jpg";
            string outputPath = "EmbeddedImage_out.msg";

            // Ensure the image file exists; create a minimal placeholder if missing
            if (!File.Exists(imagePath))
            {
                try
                {
                    using (FileStream fs = File.Create(imagePath))
                    {
                        // Minimal JPEG header (empty image)
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

            using (MailMessage message = new MailMessage())
            {
                message.From = "AndrewIrwin@from.com";
                message.To.Add("SusanMarc@to.com");
                message.Subject = "This is an email";

                AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                    "This is my plain text content", null, "text/plain");

                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                    "Here is an embedded image.<img src=cid:barcode>", null, "text/html");

                // Create the linked resource (embedded image) and set its Content-Id
                LinkedResource barcode = new LinkedResource(imagePath, MediaTypeNames.Image.Jpeg)
                {
                    ContentId = "barcode"
                };

                // Add the linked resource and alternate views to the message
                message.LinkedResources.Add(barcode);
                message.AlternateViews.Add(plainView);
                message.AlternateViews.Add(htmlView);

                // Save the message as an MSG file
                message.Save(outputPath, SaveOptions.DefaultMsgUnicode);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
