using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define paths
            string attachmentPath = "sample.txt";
            string outputPath = "output.msg";

            // Ensure attachment file exists
            if (!File.Exists(attachmentPath))
            {
                try
                {
                    File.WriteAllText(attachmentPath, "Sample attachment content");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder attachment: {ex.Message}");
                    return;
                }
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
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

            // Create the email message
            using (MailMessage mail = new MailMessage())
            {
                mail.From = "sender@example.com";
                mail.To.Add("receiver@example.com");
                mail.Subject = "Message with attachment";
                mail.Body = "Please see the attached file.";

                // Add attachment
                using (Attachment attachment = new Attachment(attachmentPath))
                {
                    mail.Attachments.Add(attachment);
                }

                // Save as MSG
                try
                {
                    mail.Save(outputPath, new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat));
                    Console.WriteLine($"Message saved to {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
