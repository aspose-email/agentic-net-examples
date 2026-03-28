using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string msgPath = "input.msg";
            string outputDirectory = "ExtractedAttachments";

            // Verify input MSG file exists
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


            // Ensure output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load MSG with TNEF attachment extraction enabled
            MsgLoadOptions loadOptions = new MsgLoadOptions
            {
                PreserveTnefAttachments = true
            };

            using (MailMessage message = MailMessage.Load(msgPath, loadOptions))
            {
                foreach (Attachment attachment in message.Attachments)
                {
                    using (Attachment att = attachment)
                    {
                        string outputPath = Path.Combine(outputDirectory, att.Name);
                        using (FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                        {
                            att.ContentStream.CopyTo(fileStream);
                        }
                        Console.WriteLine($"Extracted: {outputPath}");
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
