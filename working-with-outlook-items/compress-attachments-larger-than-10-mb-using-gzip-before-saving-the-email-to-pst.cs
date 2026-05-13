using Aspose.Email.Mapi;
using System;
using System.IO;
using System.IO.Compression;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for the attachment and the PST file
            string attachmentPath = "largefile.bin";
            string pstPath = "output.pst";

            // Ensure the attachment file exists; create a placeholder if missing
            if (!File.Exists(attachmentPath))
            {
                try
                {
                    using (FileStream placeholderStream = File.Create(attachmentPath))
                    {
                        byte[] placeholderData = new byte[1024]; // 1 KB placeholder
                        placeholderStream.Write(placeholderData, 0, placeholderData.Length);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder attachment: {ex.Message}");
                    return;
                }
            }

            // Create a simple mail message
            using (MailMessage mailMessage = new MailMessage())
            {
                mailMessage.From = "sender@example.com";
                mailMessage.To.Add("recipient@example.com");
                mailMessage.Subject = "Test Email with Attachment";
                mailMessage.Body = "Please see the attached file.";

                // Load attachment bytes
                byte[] attachmentBytes;
                try
                {
                    attachmentBytes = File.ReadAllBytes(attachmentPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to read attachment file: {ex.Message}");
                    return;
                }

                const long tenMegabytes = 10L * 1024L * 1024L;
                if (attachmentBytes.LongLength > tenMegabytes)
                {
                    // Compress attachment using GZIP
                    using (MemoryStream compressedStream = new MemoryStream())
                    {
                        try
                        {
                            using (GZipStream gzipStream = new GZipStream(compressedStream, CompressionMode.Compress, true))
                            {
                                gzipStream.Write(attachmentBytes, 0, attachmentBytes.Length);
                            }
                            compressedStream.Position = 0;
                            string compressedFileName = Path.GetFileName(attachmentPath) + ".gz";
                            Attachment compressedAttachment = new Attachment(compressedStream, compressedFileName);
                            mailMessage.Attachments.Add(compressedAttachment);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to compress attachment: {ex.Message}");
                            return;
                        }
                    }
                }
                else
                {
                    // Add original attachment without compression
                    using (MemoryStream originalStream = new MemoryStream(attachmentBytes))
                    {
                        Attachment originalAttachment = new Attachment(originalStream, Path.GetFileName(attachmentPath));
                        mailMessage.Attachments.Add(originalAttachment);
                    }
                }

                // Ensure the PST file path is ready
                try
                {
                    if (File.Exists(pstPath))
                    {
                        // Overwrite existing PST by deleting it
                        File.Delete(pstPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to prepare PST file: {ex.Message}");
                    return;
                }

                // Create PST and add the message
                try
                {
                    using (PersonalStorage pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        pst.RootFolder.AddMessage(MapiMessage.FromMailMessage(mailMessage));
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST or add message: {ex.Message}");
                    return;
                }

                Console.WriteLine("Email saved to PST with attachment handling completed.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
