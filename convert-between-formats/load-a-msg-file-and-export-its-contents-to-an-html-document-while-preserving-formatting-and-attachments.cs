using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "sample.msg";
            string outputHtmlPath = "sample.html";
            string attachmentsDir = "Attachments";

            // Ensure the input MSG file exists; create a minimal placeholder if missing.
            if (!File.Exists(inputPath))
            {
                MailMessage placeholder = new MailMessage(
                    "sender@example.com",
                    "receiver@example.com",
                    "Placeholder",
                    "This is a placeholder message.");

                MsgSaveOptions msgSaveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat);
                placeholder.Save(inputPath, msgSaveOptions);
            }

            // Ensure the attachments directory exists.
            if (!Directory.Exists(attachmentsDir))
            {
                Directory.CreateDirectory(attachmentsDir);
            }

            // Load the MSG file.
            using (MailMessage message = MailMessage.Load(inputPath, new MsgLoadOptions()))
            {
                // Save the message as HTML with embedded resources.
                HtmlSaveOptions htmlOptions = new HtmlSaveOptions
                {
                    ResourceRenderingMode = ResourceRenderingMode.EmbedIntoHtml
                };
                message.Save(outputHtmlPath, htmlOptions);

                // Extract attachments to the designated folder.
                foreach (Attachment attachment in message.Attachments)
                {
                    string attachmentName = string.IsNullOrEmpty(attachment.Name) ? "attachment" : attachment.Name;
                    string safeName = GetSafeFileName(attachmentName);
                    string attachmentPath = Path.Combine(attachmentsDir, safeName);

                    using (FileStream fs = new FileStream(attachmentPath, FileMode.Create, FileAccess.Write))
                    {
                        attachment.ContentStream.CopyTo(fs);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Helper to sanitize file names.
    static string GetSafeFileName(string fileName)
    {
        foreach (char c in Path.GetInvalidFileNameChars())
        {
            fileName = fileName.Replace(c, '_');
        }
        return fileName;
    }
}
