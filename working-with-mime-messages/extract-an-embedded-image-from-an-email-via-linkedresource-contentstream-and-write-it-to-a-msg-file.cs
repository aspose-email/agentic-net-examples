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
            string emlPath = "sample.eml";
            string outputMsgPath = "extractedImage.msg";

            if (!File.Exists(emlPath))
            {
                Console.Error.WriteLine($"Error: File not found – {emlPath}");
                return;
            }

            using (MailMessage eml = MailMessage.Load(emlPath))
            {
                if (eml.LinkedResources.Count == 0)
                {
                    Console.Error.WriteLine("No linked resources found in the email.");
                    return;
                }

                LinkedResource resource = eml.LinkedResources[0];

                using (Stream resourceStream = resource.ContentStream)
                using (MemoryStream memory = new MemoryStream())
                {
                    resourceStream.CopyTo(memory);
                    memory.Position = 0;

                    using (MailMessage newMessage = new MailMessage())
                    {
                        // Create an attachment from the extracted image stream
                        ContentType attachmentType = new ContentType(resource.ContentType.MediaType);
                        Attachment attachment = new Attachment(memory, attachmentType);
                        newMessage.Attachments.Add(attachment);

                        // Save the new message (with the image as attachment) as MSG
                        newMessage.Save(outputMsgPath);
                    }
                }
            }

            Console.WriteLine($"Embedded image extracted and saved to {outputMsgPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
