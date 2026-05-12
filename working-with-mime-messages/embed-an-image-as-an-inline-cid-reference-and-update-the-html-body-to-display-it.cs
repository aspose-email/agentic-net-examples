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
            // Paths for the image and output message
            string imagePath = "image.jpg";
            string outputMessagePath = "EmbeddedImage.msg";

            // Verify that the image file exists before proceeding
            if (!File.Exists(imagePath))
            {
                Console.Error.WriteLine($"Image file not found: {imagePath}");
                return;
            }

            // Create a new email message
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Email with inline image";

                // Plain‑text view (optional, for clients that do not support HTML)
                using (AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                    "This email contains an embedded image.", null, "text/plain"))
                {
                    message.AlternateViews.Add(plainView);
                }

                // HTML view with CID reference to the image
                string htmlBody = "<html><body><h3>Embedded Image</h3><img src=\"cid:myImage\" /></body></html>";
                using (AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                    htmlBody, null, "text/html"))
                {
                    // Create the linked resource (the image) and assign a Content‑Id
                    using (LinkedResource linkedImage = new LinkedResource(imagePath, MediaTypeNames.Image.Jpeg))
                    {
                        linkedImage.ContentId = "myImage";

                        // Add the linked resource to the HTML view
                        htmlView.LinkedResources.Add(linkedImage);

                        // Add the HTML view to the message
                        message.AlternateViews.Add(htmlView);
                    }
                }

                // Save the message to a file (MSG format)
                try
                {
                    message.Save(outputMessagePath, SaveOptions.DefaultMsgUnicode);
                    Console.WriteLine($"Message saved to {outputMessagePath}");
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
