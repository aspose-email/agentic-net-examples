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
            // Prepare output path and ensure its directory exists
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
                eml.To = "SusanMarc@to.com";
                eml.Subject = "This is an email";

                // Plain text view
                AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                    "This is my plain text content", null, "text/plain");

                // HTML view with a placeholder for an embedded image
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                    "Here is an embedded image. <img src=cid:barcode>", null, "text/html");

                // Attempt to add the linked image resource if the file exists
                string imagePath = "1.jpg";
                if (File.Exists(imagePath))
                {
                    LinkedResource barcode = new LinkedResource(imagePath, MediaTypeNames.Image.Jpeg)
                    {
                        ContentId = "barcode"
                    };
                    eml.LinkedResources.Add(barcode);
                }
                else
                {
                    Console.Error.WriteLine($"Warning: Image file '{imagePath}' not found. Skipping linked resource.");
                }

                // Add alternate views to the message
                eml.AlternateViews.Add(plainView);
                eml.AlternateViews.Add(htmlView);

                // Save the message in MSG format
                eml.Save(outputPath, SaveOptions.DefaultMsgUnicode);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
