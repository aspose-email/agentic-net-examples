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
            // Paths for the inline image and the output email file
            string imagePath = "barcode.jpg";
            string outputPath = "InlineImageEmail.msg";

            // Ensure the image file exists; create an empty placeholder if it does not
            if (!File.Exists(imagePath))
            {
                try
                {
                    File.WriteAllBytes(imagePath, new byte[0]);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder image: {ex.Message}");
                    return;
                }
            }

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
            if (!Directory.Exists(outputDir))
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

            // Create the email message
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Email with Inline Image";

                // Plain‑text view (fallback for clients that do not support HTML)
                AlternateView plainView = AlternateView.CreateAlternateViewFromString(
                    "This email contains an inline image.", null, "text/plain");

                // HTML view referencing the image via CID
                string htmlBody = "Here is an inline image: <img src=\"cid:barcode\">";
                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(
                    htmlBody, null, "text/html");

                // Create the linked resource for the image and assign a Content‑Id
                using (LinkedResource imageResource = new LinkedResource(imagePath, MediaTypeNames.Image.Jpeg))
                {
                    imageResource.ContentId = "barcode";

                    // Attach the linked resource and alternate views to the message
                    message.LinkedResources.Add(imageResource);
                    message.AlternateViews.Add(plainView);
                    message.AlternateViews.Add(htmlView);

                    // Save the message to a MSG file
                    try
                    {
                        message.Save(outputPath, SaveOptions.DefaultMsgUnicode);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save email: {ex.Message}");
                        return;
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
