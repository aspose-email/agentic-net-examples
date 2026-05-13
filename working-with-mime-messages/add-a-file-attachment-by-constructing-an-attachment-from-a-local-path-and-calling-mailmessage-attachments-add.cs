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
            string outputMessagePath = "MessageWithAttachment.eml";

            // Ensure attachment file exists; create a placeholder if missing
            if (!File.Exists(attachmentPath))
            {
                try
                {
                    File.WriteAllText(attachmentPath, "Placeholder attachment content.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder attachment: {ex.Message}");
                    return;
                }
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputMessagePath);
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
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To = "receiver@example.com";
                message.Subject = "Test Email with Attachment";
                message.Body = "Please see the attached file.";

                // Add the attachment
                using (Attachment attachment = new Attachment(attachmentPath))
                {
                    message.Attachments.Add(attachment);
                }

                // Save the message to disk
                try
                {
                    message.Save(outputMessagePath);
                    Console.WriteLine($"Message saved to '{outputMessagePath}'.");
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
