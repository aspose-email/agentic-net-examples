using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string msgPath = "sample.msg";
            if (!File.Exists(msgPath))
{
    try
    {
        MailMessage placeholderMsg = new MailMessage("sender@example.com", "recipient@example.com", "Placeholder", "This is a placeholder MSG.");
        placeholderMsg.Save(msgPath, SaveOptions.DefaultMsgUnicode);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder MSG: {ex.Message}");
        return;
    }
}


            string outputDirectory = "ExtractedImages";
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                foreach (MapiAttachment attachment in message.Attachments)
                {
                    string extension = Path.GetExtension(attachment.FileName).ToLowerInvariant();
                    bool isImage = extension == ".png" ||
                                   extension == ".jpg" ||
                                   extension == ".jpeg" ||
                                   extension == ".gif" ||
                                   extension == ".bmp";

                    if (isImage)
                    {
                        string outputPath = Path.Combine(outputDirectory, attachment.FileName);
                        try
                        {
                            attachment.Save(outputPath);
                            Console.WriteLine($"Saved image: {outputPath}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to save attachment '{attachment.FileName}': {ex.Message}");
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
