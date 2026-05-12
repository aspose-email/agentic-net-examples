using System;
using System.IO;
using System.IO.Compression;
using Aspose.Email;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.eml";
            string outputPath = "output.eml";

            // Verify input file exists
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
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }
            }

            // Load the email message
            using (MailMessage mailMessage = MailMessage.Load(inputPath))
            {
                // Process attachments in reverse order to allow removal
                for (int i = mailMessage.Attachments.Count - 1; i >= 0; i--)
                {
                    Attachment attachment = mailMessage.Attachments[i];

                    // Determine attachment size (if stream is available)
                    long attachmentSize = 0;
                    if (attachment.ContentStream != null && attachment.ContentStream.CanSeek)
                    {
                        attachmentSize = attachment.ContentStream.Length;
                    }
                    else if (attachment.ContentStream != null)
                    {
                        // Copy to memory to determine size
                        using (MemoryStream tempStream = new MemoryStream())
                        {
                            attachment.ContentStream.CopyTo(tempStream);
                            attachmentSize = tempStream.Length;
                            // Reset original stream position for later use
                            attachment.ContentStream.Position = 0;
                        }
                    }

                    // Compress attachments larger than 1 MB
                    const long sizeThreshold = 1 * 1024 * 1024; // 1 MB
                    if (attachmentSize > sizeThreshold)
                    {
                        // Read original attachment data into memory
                        byte[] originalData;
                        using (MemoryStream originalStream = new MemoryStream())
                        {
                            attachment.ContentStream.CopyTo(originalStream);
                            originalData = originalStream.ToArray();
                            // Reset original stream position for safety
                            attachment.ContentStream.Position = 0;
                        }

                        // Create ZIP archive in memory
                        using (MemoryStream zipStream = new MemoryStream())
                        {
                            using (ZipArchive zipArchive = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                            {
                                string entryName = string.IsNullOrEmpty(attachment.Name) ? "attachment" : attachment.Name;
                                ZipArchiveEntry zipEntry = zipArchive.CreateEntry(entryName);
                                using (Stream entryStream = zipEntry.Open())
                                {
                                    entryStream.Write(originalData, 0, originalData.Length);
                                }
                            }

                            // Prepare ZIP attachment
                            zipStream.Position = 0;
                            string zipFileName = (string.IsNullOrEmpty(attachment.Name) ? "attachment" : attachment.Name) + ".zip";
                            Attachment zipAttachment = new Attachment(zipStream, zipFileName, "application/zip");

                            // Replace original attachment with ZIP attachment
                            mailMessage.Attachments.RemoveAt(i);
                            mailMessage.Attachments.Add(zipAttachment);
                        }
                    }
                }

                // Save the modified message
                try
                {
                    mailMessage.Save(outputPath);
                    Console.WriteLine($"Message saved to {outputPath}");
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Failed to save message: {saveEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
