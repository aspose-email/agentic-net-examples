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


            string outputDir = "Attachments";
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            using (MapiMessage message = MapiMessage.Load(msgPath))
            {
                foreach (MapiAttachment attachment in message.Attachments)
                {
                    string fileName = attachment.FileName;
                    if (string.IsNullOrEmpty(fileName))
                    {
                        fileName = "UnnamedAttachment.bin";
                    }
                    string outputPath = Path.Combine(outputDir, fileName);
                    try
                    {
                        File.WriteAllBytes(outputPath, attachment.BinaryData);
                        Console.WriteLine($"Saved attachment: {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save attachment {fileName}: {ex.Message}");
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
