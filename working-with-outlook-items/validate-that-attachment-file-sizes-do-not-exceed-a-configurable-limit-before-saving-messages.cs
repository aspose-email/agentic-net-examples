using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            const long MaxAttachmentSizeBytes = 5 * 1024 * 1024; // 5 MB limit
            string inputPath = "input.eml";
            string outputPath = "output.eml";

            // Ensure input file exists; create a minimal placeholder if missing
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

                try
                {
                    File.WriteAllText(inputPath, "Subject: Placeholder\r\n\r\nBody");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder input file: {ex.Message}");
                    return;
                }
            }

            // Ensure output directory exists
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

            // Load the message and validate attachment sizes
            try
            {
                using (MailMessage message = MailMessage.Load(inputPath))
                {
                    foreach (Attachment attachment in message.Attachments)
                    {
                        if (attachment.ContentStream != null && attachment.ContentStream.Length > MaxAttachmentSizeBytes)
                        {
                            Console.Error.WriteLine($"Attachment \"{attachment.Name}\" exceeds the size limit of {MaxAttachmentSizeBytes} bytes.");
                            return;
                        }
                    }

                    // All attachments are within the limit; save the message
                    try
                    {
                        message.Save(outputPath);
                        Console.WriteLine($"Message saved to \"{outputPath}\".");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save message: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load or process the message: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
