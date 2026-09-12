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
            const string msgPath = "sample.msg";

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

                Console.Error.WriteLine($"MSG file '{msgPath}' not found.");
                return;
            }

            MailMessage message;
            try
            {
                message = MailMessage.Load(msgPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            using (message)
            {
                const string outputDir = "Attachments";
                if (!Directory.Exists(outputDir))
                    Directory.CreateDirectory(outputDir);

                foreach (Attachment attachment in message.Attachments)
                {
                    string safeName = GetSafeFileName(attachment.Name);
                    if (string.IsNullOrEmpty(safeName))
                        safeName = "attachment";

                    string outputPath = Path.Combine(outputDir, safeName);

                    try
                    {
                        using (var fileStream = File.Create(outputPath))
                        {
                            attachment.ContentStream.CopyTo(fileStream);
                        }
                        Console.WriteLine($"Saved attachment to '{outputPath}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save attachment '{safeName}': {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    private static string GetSafeFileName(string name)
    {
        if (string.IsNullOrEmpty(name))
            return null;

        foreach (char c in Path.GetInvalidFileNameChars())
        {
            name = name.Replace(c, '_');
        }
        return name;
    }
}
