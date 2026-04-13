using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string msgPath = "sample.msg";
            string outputFolder = "InlineImages";

            // Verify input file exists
            if (!File.Exists(msgPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(msgPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Load the MSG file as a MailMessage
            using (MailMessage message = MailMessage.Load(msgPath))
            {
                int imageIndex = 0;
                foreach (Attachment attachment in message.Attachments)
                {
                    // Inline images usually have a ContentId
                    if (!string.IsNullOrEmpty(attachment.ContentId))
                    {
                        string fileName = attachment.Name;
                        if (string.IsNullOrEmpty(fileName))
                        {
                            fileName = $"inline_{imageIndex}";
                        }

                        string outputPath = Path.Combine(outputFolder, fileName);

                        // Save the attachment data to the file system
                        using (Stream contentStream = attachment.ContentStream)
                        using (FileStream fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write))
                        {
                            contentStream.CopyTo(fileStream);
                        }

                        imageIndex++;
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
