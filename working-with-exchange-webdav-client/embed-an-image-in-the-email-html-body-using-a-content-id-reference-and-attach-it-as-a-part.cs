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
            string imagePath = "image.jpg";

            // Ensure the image file exists; create a minimal placeholder if it does not
            if (!File.Exists(imagePath))
            {
                try
                {
                    using (FileStream fs = File.Create(imagePath))
                    {
                        // Placeholder content (empty file is acceptable for the example)
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder image: {ex.Message}");
                    return;
                }
            }

            // Create the email message
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Email with embedded image";

                // Plain‑text view (fallback for clients that do not support HTML)
                AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                    "This is the plain text body.", null, "text/plain");

                // HTML view with a CID reference to the embedded image
                string htmlBody = "Here is an embedded image: <img src=\"cid:myImage\" />";
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                    htmlBody, null, "text/html");

                // Embed the image as a linked resource
                using (LinkedResource linkedImage = new LinkedResource(imagePath, MediaTypeNames.Image.Jpeg))
                {
                    linkedImage.ContentId = "myImage";
                    htmlView.LinkedResources.Add(linkedImage);

                    // Add the alternate views to the message
                    message.AlternateViews.Add(plainView);
                    message.AlternateViews.Add(htmlView);

                    // Also attach the image as a regular attachment
                    using (Attachment attachment = new Attachment(imagePath, MediaTypeNames.Image.Jpeg))
                    {
                        message.Attachments.Add(attachment);

                        // Save the composed message to disk
                        string outputPath = "EmbeddedImageMessage.msg";
                        try
                        {
                            string outputDir = Path.GetDirectoryName(outputPath);
                            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                            {
                                Directory.CreateDirectory(outputDir);
                            }
                            message.Save(outputPath, SaveOptions.DefaultMsgUnicode);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                        }
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
