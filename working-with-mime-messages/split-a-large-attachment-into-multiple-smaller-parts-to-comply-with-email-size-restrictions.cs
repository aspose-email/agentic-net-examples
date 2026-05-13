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
            // Input attachment path
            string attachmentPath = "largefile.bin";

            // Ensure the input file exists; create a placeholder if missing
            if (!File.Exists(attachmentPath))
            {
                try
                {
                    byte[] placeholder = new byte[1024]; // 1 KB placeholder
                    new Random().NextBytes(placeholder);
                    File.WriteAllBytes(attachmentPath, placeholder);
                    Console.WriteLine($"Placeholder attachment created at '{attachmentPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder attachment: {ex.Message}");
                    return;
                }
            }

            // Define the maximum size for each part (e.g., 1 MB)
            long partSize = 1 * 1024 * 1024; // 1,048,576 bytes

            // Read the entire attachment into memory
            byte[] allBytes;
            try
            {
                allBytes = File.ReadAllBytes(attachmentPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to read attachment file: {ex.Message}");
                return;
            }

            // Calculate number of parts
            int totalParts = (int)Math.Ceiling((double)allBytes.Length / partSize);
            List<Attachment> attachmentParts = new List<Attachment>();

            // Split the attachment into parts
            for (int i = 0; i < totalParts; i++)
            {
                long offset = i * partSize;
                long remaining = allBytes.Length - offset;
                int currentPartSize = (int)Math.Min(partSize, remaining);
                byte[] partBytes = new byte[currentPartSize];
                Array.Copy(allBytes, offset, partBytes, 0, currentPartSize);

                // Create a memory stream for the part
                using (MemoryStream partStream = new MemoryStream(partBytes))
                {
                    string partFileName = $"{Path.GetFileNameWithoutExtension(attachmentPath)}_part{i + 1}{Path.GetExtension(attachmentPath)}";
                    Attachment partAttachment = new Attachment(partStream, partFileName);
                    attachmentParts.Add(partAttachment);
                }
            }

            // Create a mail message and add the split attachments
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Message with split attachment";
                message.Body = "The original attachment has been split into multiple parts.";

                foreach (Attachment part in attachmentParts)
                {
                    message.Attachments.Add(part);
                }

                // Ensure output directory exists
                string outputDir = "output";
                if (!Directory.Exists(outputDir))
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

                // Save the message to an EML file
                string emlPath = Path.Combine(outputDir, "SplitAttachmentMessage.eml");
                try
                {
                    message.Save(emlPath);
                    Console.WriteLine($"Message saved to '{emlPath}'.");
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
