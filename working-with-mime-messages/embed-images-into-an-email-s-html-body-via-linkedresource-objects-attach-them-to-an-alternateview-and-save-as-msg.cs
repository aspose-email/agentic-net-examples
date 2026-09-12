using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for the image to embed and the output MSG file
            string imagePath = "logo.png";
            string outputPath = "EmailWithImage.msg";

            // Verify that the image file exists
            if (!File.Exists(imagePath))
            {
                Console.Error.WriteLine($"Image file not found: {imagePath}");
                return;
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
                message.From = new MailAddress("sender@example.com");
                message.To.Add(new MailAddress("recipient@example.com"));
                message.Subject = "Email with Embedded Image";

                // HTML body referencing the embedded image via Content-ID
                string htmlBody = "<html><body><h1>Hello</h1><img src=\"cid:logoImage\" alt=\"Logo\"/></body></html>";

                // Create an AlternateView for the HTML content
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(htmlBody, null, "text/html");

                // Create a LinkedResource for the image file
                using (LinkedResource logoResource = new LinkedResource(imagePath))
                {
                    logoResource.ContentId = "logoImage";
                    logoResource.TransferEncoding = TransferEncoding.Base64;

                    // Attach the linked resource to the HTML view
                    htmlView.LinkedResources.Add(logoResource);

                    // Add the HTML view to the message
                    message.AlternateViews.Add(htmlView);

                    // Save the message as a MSG file
                    message.Save(outputPath);
                }
            }

            Console.WriteLine($"Message saved to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
