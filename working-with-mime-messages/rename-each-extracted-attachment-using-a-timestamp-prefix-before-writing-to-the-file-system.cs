using System;
using System.IO;
using Aspose.Email;

namespace AttachmentRenameExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Path to the source EML file
                string emlPath = "sample.eml";
                // Directory where renamed attachments will be saved
                string outputDirectory = "ExtractedAttachments";

                // Verify that the source file exists
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

                    Console.Error.WriteLine($"Input file '{emlPath}' not found. Skipping extraction.");
                    return;
                }

                // Ensure the output directory exists
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory '{outputDirectory}': {dirEx.Message}");
                    return;
                }

                // Load the email message
                using (MailMessage message = MailMessage.Load(emlPath))
                {
                    // Iterate through each attachment
                    foreach (Attachment attachment in message.Attachments)
                    {
                        // Build a timestamp prefix
                        string timestamp = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                        // Use the original attachment name if available
                        string originalName = attachment.Name ?? "attachment";
                        // Construct the new filename with the timestamp prefix
                        string newFileName = $"{timestamp}_{originalName}";
                        string outputPath = Path.Combine(outputDirectory, newFileName);

                        // Save the attachment with the new name
                        try
                        {
                            attachment.Save(outputPath);
                            Console.WriteLine($"Saved attachment as '{outputPath}'.");
                        }
                        catch (Exception saveEx)
                        {
                            Console.Error.WriteLine($"Failed to save attachment '{originalName}': {saveEx.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
