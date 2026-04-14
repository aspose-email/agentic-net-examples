using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define paths
            string imagePath = "image.png";
            string outputPath = "output.mht";

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // HTML body with reference to the embedded image
            string htmlBody = "<html><body><h1>Hello, World!</h1><img src=\"cid:image1\" alt=\"Embedded Image\"/></body></html>";

            // Create the mail message
            using (MailMessage message = new MailMessage("sender@example.com", "receiver@example.com", "Sample MHT with Embedded Image", string.Empty))
            {
                message.HtmlBody = htmlBody;

                // Add the image as an inline attachment if it exists
                if (File.Exists(imagePath))
                {
                    Attachment inlineAttachment = new Attachment(imagePath);
                    inlineAttachment.ContentId = "image1";
                    // Mark the attachment as inline
                    inlineAttachment.ContentDisposition.Inline = true;
                    message.Attachments.Add(inlineAttachment);
                }
                else
                {
                    Console.Error.WriteLine($"Image file not found: {imagePath}. The message will be saved without the embedded image.");
                }

                // Configure MHT save options to retain attachments
                MhtSaveOptions saveOptions = new MhtSaveOptions
                {
                    SaveAttachments = true,
                    SkipInlineImages = false
                };

                // Save the message as MHT
                message.Save(outputPath, saveOptions);
                Console.WriteLine($"Message saved successfully to '{outputPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
