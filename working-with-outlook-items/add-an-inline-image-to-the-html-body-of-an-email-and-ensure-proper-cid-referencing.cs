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
            // Path to the image that will be embedded
            string imagePath = "barcode.jpg";

            // Ensure the image file exists; create a minimal placeholder if missing
            if (!File.Exists(imagePath))
            {
                try
                {
                    File.WriteAllBytes(imagePath, new byte[0]);
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to create placeholder image: {ioEx.Message}");
                    return;
                }
            }

            // Create the email message
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To = "receiver@example.com";
                message.Subject = "Email with inline image";

                // Plain text view
                AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                    "This is plain text.", null, "text/plain");

                // HTML view with CID reference to the embedded image
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                    "Here is an embedded image: <img src=\"cid:barcode\">", null, "text/html");

                // Linked resource representing the image
                LinkedResource linkedImage = new LinkedResource(imagePath, MediaTypeNames.Image.Jpeg)
                {
                    ContentId = "barcode"
                };

                // Attach resources and views to the message
                message.LinkedResources.Add(linkedImage);
                message.AlternateViews.Add(plainView);
                message.AlternateViews.Add(htmlView);

                // Output file path
                string outputPath = "EmbeddedImage_out.msg";

                // Ensure the output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    try
                    {
                        Directory.CreateDirectory(outputDir);
                    }
                    catch (Exception dirEx)
                    {
                        Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                        return;
                    }
                }

                // Save the message
                try
                {
                    message.Save(outputPath, SaveOptions.DefaultMsgUnicode);
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Failed to save message: {saveEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
