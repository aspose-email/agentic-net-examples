using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "sample.eml";
            string outputFolder = "ImageAttachments";

            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            using (MailMessage message = MailMessage.Load(inputPath))
            {
                foreach (Attachment attachment in message.Attachments)
                {
                    string mediaType = attachment.ContentType?.MediaType;
                    if (mediaType != null && mediaType.StartsWith("image/", StringComparison.OrdinalIgnoreCase))
                    {
                        string attachmentName = attachment.Name;
                        if (string.IsNullOrEmpty(attachmentName))
                        {
                            attachmentName = "unnamed_image";
                        }

                        string outputPath = Path.Combine(outputFolder, attachmentName);
                        try
                        {
                            attachment.Save(outputPath);
                            Console.WriteLine($"Saved image attachment: {outputPath}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save attachment '{attachmentName}': {ex.Message}");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
