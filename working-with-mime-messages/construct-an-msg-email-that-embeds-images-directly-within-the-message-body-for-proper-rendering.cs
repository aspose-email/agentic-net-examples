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
            // Define output MSG file path
            string outputMsgPath = "EmbeddedImage.msg";

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputMsgPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create a new mail message
            using (MailMessage message = new MailMessage("sender@example.com", "recipient@example.com", "Test email with embedded image", string.Empty))
            {
                // Enable HTML body
                message.IsBodyHtml = true;

                // HTML body referencing the embedded image via Content-ID
                string htmlBody = "<html><body><h1>Hello</h1><img src=\"cid:image1\"></body></html>";
                message.HtmlBody = htmlBody;

                // Prepare a minimal placeholder PNG (1x1 pixel) if the image file is missing
                byte[] imageBytes = Convert.FromBase64String(
                    "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mP8/5+hHgAFgwJ/lKXK5wAAAABJRU5ErkJggg==");

                // Add the image as a linked resource with a Content-ID matching the HTML reference
                using (MemoryStream imageStream = new MemoryStream(imageBytes))
                {
                    LinkedResource resource = new LinkedResource(imageStream, "image/png")
                    {
                        ContentId = "image1"
                    };
                    message.LinkedResources.Add(resource);
                }

                // Save the message as MSG with preserved original dates
                MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode)
                {
                    PreserveOriginalDates = true
                };
                message.Save(outputMsgPath, saveOptions);
            }

            Console.WriteLine("MSG file created successfully: " + outputMsgPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
