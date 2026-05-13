using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Prepare JSON metadata
            string jsonContent = "{\"author\":\"John Doe\",\"version\":1}";
            // Create attachment from string with JSON content type
            Attachment jsonAttachment = Attachment.CreateAttachmentFromString(jsonContent, "application/json");
            jsonAttachment.Name = "metadata.json";

            // Create a simple mail message
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To = "receiver@example.com";
                message.Subject = "Message with JSON attachment";
                message.Body = "Please see the attached JSON metadata.";

                // Add the JSON attachment
                message.Attachments.Add(jsonAttachment);

                // Define output path
                string outputPath = "MessageWithJsonAttachment.eml";
                string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Save the message to disk with file IO guard
                try
                {
                    message.Save(outputPath);
                    Console.WriteLine($"Message saved to '{outputPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                    return;
                }
            }

            // Dispose attachment explicitly if not already disposed by MailMessage
            jsonAttachment.Dispose();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
