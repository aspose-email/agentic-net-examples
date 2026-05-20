using System;
using System.IO;
using System.IO.Compression;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // SMTP configuration (replace with real values)
            string smtpHost = "smtp.example.com";
            int smtpPort = 587;
            string username = "user@example.com";
            string password = "password";

            // Skip execution if placeholder configuration is detected
            if (smtpHost.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping send.");
                return;
            }

            // Create the email message
            MailMessage message = new MailMessage();
            message.From = "sender@example.com";
            message.To.Add("receiver@example.com");
            message.Subject = "Test email with compressed attachment";
            message.Body = "Please see the attached file.";

            // Path to the original attachment
            string originalFilePath = "largefile.dat";

            // Ensure temporary directory exists for compressed files
            string tempDir = Path.Combine(Path.GetTempPath(), "AsposeEmailTemp");
            if (!Directory.Exists(tempDir))
            {
                Directory.CreateDirectory(tempDir);
            }

            string attachmentPath = originalFilePath;
            string tempCompressedPath = null;

            try
            {
                if (File.Exists(originalFilePath))
                {
                    FileInfo fileInfo = new FileInfo(originalFilePath);
                    // Compress if larger than 1 MB
                    if (fileInfo.Length > 1 * 1024 * 1024)
                    {
                        tempCompressedPath = Path.Combine(tempDir, fileInfo.Name + ".gz");
                        using (FileStream originalStream = File.OpenRead(originalFilePath))
                        using (FileStream compressedStream = File.Create(tempCompressedPath))
                        using (GZipStream gzip = new GZipStream(compressedStream, CompressionMode.Compress))
                        {
                            originalStream.CopyTo(gzip);
                        }
                        attachmentPath = tempCompressedPath;
                    }
                }
                else
                {
                    Console.Error.WriteLine($"Attachment file not found: {originalFilePath}");
                }

                // Add the attachment to the message
                Attachment attachment = new Attachment(attachmentPath);
                if (attachmentPath.EndsWith(".gz", StringComparison.OrdinalIgnoreCase))
                {
                    attachment.ContentType.MediaType = "application/gzip";
                    attachment.Name = Path.GetFileName(originalFilePath);
                }
                message.Attachments.Add(attachment);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing attachment: {ex.Message}");
                return;
            }

            // Send the email
            try
            {
                using (SmtpClient client = new SmtpClient(smtpHost, smtpPort, username, password))
                {
                    client.Send(message);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error sending email: {ex.Message}");
                return;
            }
            finally
            {
                // Clean up temporary compressed file
                if (tempCompressedPath != null && File.Exists(tempCompressedPath))
                {
                    try
                    {
                        File.Delete(tempCompressedPath);
                    }
                    catch
                    {
                        // Ignore cleanup errors
                    }
                }
                // Dispose the MailMessage
                message.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
