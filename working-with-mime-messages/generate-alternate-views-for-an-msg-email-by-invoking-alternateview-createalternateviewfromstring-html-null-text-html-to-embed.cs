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
            // Output file path
            string outputPath = "EmbeddedImage_out.msg";

            // Ensure the output directory exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create the email message
            using (MailMessage eml = new MailMessage())
            {
                eml.From = "AndrewIrwin@from.com";
                eml.To.Add("SusanMarc@to.com");
                eml.Subject = "This is an email";

                // Plain text alternate view
                AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                    "This is my plain text content", null, "text/plain");

                // HTML alternate view with embedded image reference
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                    "Here is an embedded image. <img src=cid:barcode>", null, "text/html");

                // Linked resource (embedded image)
                LinkedResource barcode = new LinkedResource("1.jpg", MediaTypeNames.Image.Jpeg)
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
            // Output any errors to the error stream
            Console.Error.WriteLine(ex.Message);
        }
    }
}
