using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace ExtractEmbeddedImages
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string msgFilePath = "sample.msg";
                string outputDirectory = "ExtractedImages";

                if (!File.Exists(msgFilePath))
{
    try
    {
        MailMessage placeholderMsg = new MailMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder MSG.");
        placeholderMsg.Save(msgFilePath, SaveOptions.DefaultMsgUnicode);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
        return;
    }
}


                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                using (MapiMessage message = MapiMessage.Load(msgFilePath))
                {
                    foreach (MapiAttachment attachment in message.Attachments)
                    {
                        string attachmentFileName = attachment.FileName;
                        if (string.IsNullOrEmpty(attachmentFileName))
                        {
                            continue;
                        }

                        string fileExtension = Path.GetExtension(attachmentFileName).ToLowerInvariant();
                        bool isImage = fileExtension == ".png" ||
                                       fileExtension == ".jpg" ||
                                       fileExtension == ".jpeg" ||
                                       fileExtension == ".gif" ||
                                       fileExtension == ".bmp" ||
                                       fileExtension == ".tiff";

                        if (!isImage)
                        {
                            continue;
                        }

                        string outputPath = Path.Combine(outputDirectory, attachmentFileName);
                        attachment.Save(outputPath);
                        Console.WriteLine($"Saved image: {outputPath}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
