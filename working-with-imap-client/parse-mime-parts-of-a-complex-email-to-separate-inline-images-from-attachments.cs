using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Path to the source EML file
            string emlPath = "sample.eml";

            // Verify that the EML file exists before attempting to load it
            if (!File.Exists(emlPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"The file '{emlPath}' does not exist.");
                return;
            }

            // Load the email message inside a using block to ensure proper disposal
            using (MailMessage message = MailMessage.Load(emlPath))
            {
                // Prepare collections for inline images and regular attachments
                List<Attachment> inlineImages = new List<Attachment>();
                List<Attachment> regularAttachments = new List<Attachment>();

                // Separate attachments based on the presence of a Content-Id (inline images have it)
                foreach (Attachment attachment in message.Attachments)
                {
                    if (!string.IsNullOrEmpty(attachment.ContentId))
                    {
                        inlineImages.Add(attachment);
                    }
                    else
                    {
                        regularAttachments.Add(attachment);
                    }
                }

                // Define output directories
                string inlineDir = "InlineImages";
                string attachmentDir = "Attachments";

                try
                {
                    // Ensure output directories exist
                    if (!Directory.Exists(inlineDir))
                    {
                        Directory.CreateDirectory(inlineDir);
                    }
                    if (!Directory.Exists(attachmentDir))
                    {
                        Directory.CreateDirectory(attachmentDir);
                    }
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directories: {dirEx.Message}");
                    return;
                }

                // Save inline images
                foreach (Attachment inline in inlineImages)
                {
                    try
                    {
                        string safeFileName = Path.GetFileName(inline.Name);
                        if (string.IsNullOrEmpty(safeFileName))
                        {
                            safeFileName = $"inline_{Guid.NewGuid()}.dat";
                        }
                        string outputPath = Path.Combine(inlineDir, safeFileName);
                        inline.Save(outputPath);
                        Console.WriteLine($"Saved inline image: {outputPath}");
                    }
                    catch (Exception saveEx)
                    {
                        Console.Error.WriteLine($"Failed to save inline image '{inline.Name}': {saveEx.Message}");
                    }
                }

                // Save regular attachments
                foreach (Attachment attach in regularAttachments)
                {
                    try
                    {
                        string safeFileName = Path.GetFileName(attach.Name);
                        if (string.IsNullOrEmpty(safeFileName))
                        {
                            safeFileName = $"attachment_{Guid.NewGuid()}.dat";
                        }
                        string outputPath = Path.Combine(attachmentDir, safeFileName);
                        attach.Save(outputPath);
                        Console.WriteLine($"Saved attachment: {outputPath}");
                    }
                    catch (Exception saveEx)
                    {
                        Console.Error.WriteLine($"Failed to save attachment '{attach.Name}': {saveEx.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
