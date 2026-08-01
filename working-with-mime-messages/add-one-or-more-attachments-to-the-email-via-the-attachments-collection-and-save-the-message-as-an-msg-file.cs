using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Author note: Example demonstrates adding multiple attachments to a MailMessage and saving as MSG.
            string outputPath = "AddAttachments.msg";

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Prepare attachment file paths
            string[] attachmentFiles = { "1.txt", "1.jpg", "1.doc", "1.rar", "1.pdf" };

            // Verify each attachment file exists; create a minimal placeholder if missing
            foreach (string filePath in attachmentFiles)
            {
                try
                {
                    if (!File.Exists(filePath))
                    {
                        string ext = Path.GetExtension(filePath).ToLowerInvariant();
                        if (ext == ".txt")
                        {
                            File.WriteAllText(filePath, "Placeholder content");
                        }
                        else
                        {
                            // Write a few zero bytes for binary placeholders
                            File.WriteAllBytes(filePath, new byte[] { 0x00, 0x01, 0x02 });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to prepare attachment '{filePath}': {ex.Message}");
                    return;
                }
            }

            // Create the email message
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@from.com";
                message.To = "receiver@to.com";
                message.Subject = "This is message";
                message.Body = "This is body";

                // Load and add the first attachment via the Attachments collection
                Attachment firstAttachment = new Attachment("1.txt");
                message.Attachments.Add(firstAttachment);

                // Add remaining attachments using AddAttachment method
                message.AddAttachment(new Attachment("1.jpg"));
                message.AddAttachment(new Attachment("1.doc"));
                message.AddAttachment(new Attachment("1.rar"));
                message.AddAttachment(new Attachment("1.pdf"));

                // Save the message as MSG
                try
                {
                    message.Save(outputPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
