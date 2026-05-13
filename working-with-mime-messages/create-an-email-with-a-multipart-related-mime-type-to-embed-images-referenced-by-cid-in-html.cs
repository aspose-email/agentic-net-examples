using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            string imagePath = "barcode.jpg";

            // Ensure the image file exists; create a minimal placeholder if missing.
            if (!File.Exists(imagePath))
            {
                try
                {
                    byte[] placeholder = new byte[]
                    {
                        0xFF,0xD8,0xFF,0xE0,0x00,0x10,0x4A,0x46,0x49,0x46,0x00,0x01,0x01,0x00,0x00,0x01,
                        0x00,0x01,0x00,0x00,0xFF,0xDB,0x00,0x43,0x00,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,
                        0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,
                        0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,
                        0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,0xFF,
                        0xFF,0xD9
                    };
                    File.WriteAllBytes(imagePath, placeholder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder image: {ex.Message}");
                    return;
                }
            }

            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("receiver@example.com");
                message.Subject = "Email with embedded image";

                string htmlContent = "<html><body><h1>Hello</h1><img src='cid:barcode'></body></html>";

                // Create the HTML alternate view.
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                    htmlContent,
                    Encoding.UTF8,
                    "text/html");

                // Create the linked resource for the image and add it to the view.
                LinkedResource linkedImage = new LinkedResource(imagePath, Aspose.Email.Mime.MediaTypeNames.Image.Jpeg);
                linkedImage.ContentId = "barcode";
                htmlView.LinkedResources.Add(linkedImage);

                // Attach the view to the message.
                message.AlternateViews.Add(htmlView);

                string outputPath = "EmbeddedImage_out.msg";

                // Ensure the output directory exists.
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

                // Save the message.
                try
                {
                    message.Save(outputPath, SaveOptions.DefaultMsgUnicode);
                    Console.WriteLine($"Message saved to {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
