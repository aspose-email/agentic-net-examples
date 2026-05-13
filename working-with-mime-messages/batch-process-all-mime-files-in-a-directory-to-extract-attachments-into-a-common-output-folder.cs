using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input directory containing MIME (.eml) files
            string inputDirectory = "MimeFiles";
            // Output directory for extracted attachments
            string outputDirectory = "ExtractedAttachments";

            // Verify input directory exists
            if (!Directory.Exists(inputDirectory))
            {
                Console.Error.WriteLine($"Input directory does not exist: {inputDirectory}");
                return;
            }

            // Ensure output directory exists
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            // Get all .eml files in the input directory
            string[] mimeFiles;
            try
            {
                mimeFiles = Directory.GetFiles(inputDirectory, "*.eml");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to enumerate files: {ex.Message}");
                return;
            }

            foreach (string mimeFilePath in mimeFiles)
            {
                // Guard against missing file (should not happen after enumeration)
                if (!File.Exists(mimeFilePath))
                {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(mimeFilePath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"File not found, skipping: {mimeFilePath}");
                    continue;
                }

                // Load the MIME message
                MailMessage mailMessage;
                try
                {
                    mailMessage = MailMessage.Load(mimeFilePath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to load MIME file '{mimeFilePath}': {ex.Message}");
                    continue;
                }

                // Process attachments
                foreach (Attachment attachment in mailMessage.Attachments)
                {
                    // Build a unique file name: originalMessageFileName_attachmentFileName
                    string safeMessageName = Path.GetFileNameWithoutExtension(mimeFilePath);
                    string safeAttachmentName = attachment.Name ?? "attachment";
                    string outputFilePath = Path.Combine(outputDirectory, $"{safeMessageName}_{safeAttachmentName}");

                    // Ensure the directory for the output file exists (already ensured globally)
                    try
                    {
                        using (FileStream fileStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write))
                        {
                            attachment.ContentStream.CopyTo(fileStream);
                        }
                        Console.WriteLine($"Saved attachment to: {outputFilePath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save attachment '{attachment.Name}' from '{mimeFilePath}': {ex.Message}");
                    }
                }

                // Dispose the MailMessage
                mailMessage.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
