using System;
using System.IO;
using System.Security.Cryptography;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "sample.eml";
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

            string outputPath = "output.eml";

            using (MailMessage message = MailMessage.Load(inputPath))
            {
                foreach (Attachment attachment in message.Attachments)
                {
                    using (Stream attachmentStream = attachment.ContentStream)
                    {
                        using (SHA256 sha256 = SHA256.Create())
                        {
                            byte[] hashBytes = sha256.ComputeHash(attachmentStream);
                            string checksum = BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
                            string headerName = $"X-Attachment-Checksum-{attachment.Name}";
                            message.Headers.Add(headerName, checksum);
                        }
                    }
                }

                try
                {
                    message.Save(outputPath);
                    Console.WriteLine($"Message saved with checksum headers to: {outputPath}");
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
