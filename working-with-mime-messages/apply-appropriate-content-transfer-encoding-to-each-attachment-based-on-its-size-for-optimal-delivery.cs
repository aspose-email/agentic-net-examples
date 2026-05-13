using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // Define attachment file paths
            string[] attachmentPaths = new string[]
            {
                "small.txt",
                "large.bin"
            };

            // Ensure attachment files exist; create minimal placeholders if missing
            foreach (string path in attachmentPaths)
            {
                try
                {
                    if (!File.Exists(path))
                    {
                        // Create a placeholder file (1 KB for small, 5 MB for large)
                        int size = path.Contains("large") ? 5 * 1024 * 1024 : 1024;
                        byte[] data = new byte[size];
                        new Random().NextBytes(data);
                        File.WriteAllBytes(path, data);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to prepare attachment '{path}': {ex.Message}");
                    return;
                }
            }

            // Create a new mail message
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Message with optimized attachments";
                message.Body = "Please see the attached files.";

                // Add attachments with optimal TransferEncoding based on size
                foreach (string path in attachmentPaths)
                {
                    try
                    {
                        using (FileStream fileStream = File.OpenRead(path))
                        {
                            Attachment attachment = new Attachment(fileStream, Path.GetFileName(path));
                            // Determine size and set appropriate TransferEncoding
                            const long LargeThreshold = 1024 * 1024; // 1 MB
                            if (fileStream.Length > LargeThreshold)
                            {
                                attachment.TransferEncoding = TransferEncoding.Base64;
                            }
                            else
                            {
                                attachment.TransferEncoding = TransferEncoding.QuotedPrintable;
                            }
                            message.Attachments.Add(attachment);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to add attachment '{path}': {ex.Message}");
                        // Continue with remaining attachments
                    }
                }

                // Save the message to an EML file
                string outputPath = "output.eml";
                try
                {
                    // Ensure the directory exists
                    string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
                    if (!Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }

                    message.Save(outputPath);
                    Console.WriteLine($"Message saved to '{outputPath}'.");
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
