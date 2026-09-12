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
            // Define output file path
            string outputPath = "EmbeddedImage_out.msg";

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Ensure the linked image exists (create a minimal placeholder if missing)
            string imagePath = "1.jpg";
            if (!File.Exists(imagePath))
            {
                // Create an empty JPEG file as a placeholder
                File.WriteAllBytes(imagePath, new byte[0]);
            }

            // Create the email message
            using (MailMessage message = new MailMessage())
            {
                message.From = "AndrewIrwin@from.com";
                message.To = "SusanMarc@to.com";
                message.Subject = "This is an email";

                // Plain text view
                AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                    "This is my plain text content", null, "text/plain");

                // HTML view with embedded image reference (cid:barcode)
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                    "Here is an embedded image. <img src=cid:barcode>", null, "text/html");

                // Linked resource for the embedded image
                LinkedResource barcode = new LinkedResource(imagePath, MediaTypeNames.Image.Jpeg)
                {
                    ContentId = "barcode"
                };

                // Attach resources and views to the message
                message.LinkedResources.Add(barcode);
                message.AlternateViews.Add(plainView);
                message.AlternateViews.Add(htmlView);

                // Save the message as MSG with Unicode encoding
                message.Save(outputPath, SaveOptions.DefaultMsgUnicode);
            }

            Console.WriteLine("Message saved successfully to " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
