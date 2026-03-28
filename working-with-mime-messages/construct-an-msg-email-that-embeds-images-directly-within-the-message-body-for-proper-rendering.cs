using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mime;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Prepare output MSG file path
            string outputMsgPath = "EmbeddedImage.msg";
            string outputDirectory = Path.GetDirectoryName(Path.GetFullPath(outputMsgPath));
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create a new MailMessage
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Email with Embedded Image";

                // HTML body referencing the embedded image via Content-ID
                string contentId = "image1";
                message.HtmlBody = $"<html><body><h1>Hello</h1><img src=\"cid:{contentId}\" alt=\"Embedded Image\"/></body></html>";
                message.IsBodyHtml = true;

                // Create a simple PNG image in memory (1x1 pixel, red)
                byte[] pngBytes = Convert.FromBase64String(
                    "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAIAAACQd1PeAAAADUlEQVR4nGMAAQAABQABDQottAAAAABJRU5ErkJggg==");

                using (MemoryStream imageStream = new MemoryStream(pngBytes))
                {
                    // Create a linked resource for the image
                    LinkedResource linkedImage = new LinkedResource(imageStream, "image/png")
                    {
                        ContentId = contentId,
                        TransferEncoding = Aspose.Email.Mime.TransferEncoding.Base64
                    };

                    // Add the linked resource to the message
                    message.LinkedResources.Add(linkedImage);
                }

                // Save the message as MSG
                message.Save(outputMsgPath, SaveOptions.DefaultMsg);
            }

            Console.WriteLine("MSG file created successfully: " + Path.GetFullPath(outputMsgPath));
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
