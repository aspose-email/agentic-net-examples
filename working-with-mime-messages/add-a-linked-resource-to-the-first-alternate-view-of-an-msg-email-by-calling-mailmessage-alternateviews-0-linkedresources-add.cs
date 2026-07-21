using System;
using System.IO;
using System.Net.Mime;
using Aspose.Email;
using Aspose.Email.Tools; // for SaveOptions if needed

class Program
{
    static void Main()
    {
        try
        {
            // Define paths
            string imagePath = "barcode.jpg";
            string outputPath = "EmbeddedImage_out.msg";

            // Verify the image file exists
            if (!File.Exists(imagePath))
            {
                Console.Error.WriteLine($"Image file not found: {imagePath}");
                return;
            }

            // Ensure output directory exists
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

                // HTML view with a placeholder for the embedded image
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                    "Here is an embedded image. <img src=cid:barcode>", null, "text/html");

                // Add alternate views to the message
                eml.AlternateViews.Add(plainView);
                eml.AlternateViews.Add(htmlView);

                // Add a linked resource to the first alternate view (index 0)
                LinkedResource linkedRes = new LinkedResource(imagePath)
                {
                    ContentId = "barcode"
                };
                eml.AlternateViews[0].LinkedResources.Add(linkedRes);

                // Save the message as MSG
                eml.Save(outputPath, SaveOptions.DefaultMsgUnicode);
            }

            Console.WriteLine("Message saved successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
