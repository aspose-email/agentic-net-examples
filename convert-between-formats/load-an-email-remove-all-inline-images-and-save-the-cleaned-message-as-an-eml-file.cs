using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.eml";
            string outputPath = "cleaned.eml";

            // Guard input file existence
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            try
            {
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {dirEx.Message}");
                return;
            }

            // Load the email message
            using (MailMessage mailMessage = MailMessage.Load(inputPath))
            {
                // Collect inline attachments (images) to remove
                List<Attachment> inlineAttachments = new List<Attachment>();
                foreach (Attachment attachment in mailMessage.Attachments)
                {
                    // Inline attachments usually have a ContentId or a disposition type of "inline"
                    if (!string.IsNullOrEmpty(attachment.ContentId) ||
                        (attachment.ContentDisposition != null &&
                         string.Equals(attachment.ContentDisposition.DispositionType, "inline", StringComparison.OrdinalIgnoreCase)))
                    {
                        inlineAttachments.Add(attachment);
                    }
                }

                // Remove collected inline attachments
                foreach (Attachment inlineAttachment in inlineAttachments)
                {
                    mailMessage.Attachments.Remove(inlineAttachment);
                }

                // Save the cleaned message
                try
                {
                    mailMessage.Save(outputPath);
                    Console.WriteLine($"Cleaned email saved to: {outputPath}");
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Failed to save cleaned email: {saveEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
