using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the attachment file
            string attachmentPath = "sample.txt";

            // Ensure the attachment file exists; create a minimal placeholder if missing
            if (!File.Exists(attachmentPath))
            {
                try
                {
                    File.WriteAllText(attachmentPath, "Sample attachment content");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create attachment file: {ex.Message}");
                    return;
                }
            }

            // Create the email message
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("receiver@example.com");
                message.Subject = "Message with attachment";
                message.Body = "Please see the attached file.";

                // Load and add the attachment
                using (Attachment attachment = new Attachment(attachmentPath))
                {
                    message.Attachments.Add(attachment);
                }

                // Define output MSG file path
                string outputPath = "MessageWithAttachment.msg";

                // Ensure the output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    try
                    {
                        Directory.CreateDirectory(outputDir);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                        return;
                    }
                }

                // Save the message as MSG using appropriate save options
                try
                {
                    MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat);
                    message.Save(outputPath, saveOptions);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save MSG file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
