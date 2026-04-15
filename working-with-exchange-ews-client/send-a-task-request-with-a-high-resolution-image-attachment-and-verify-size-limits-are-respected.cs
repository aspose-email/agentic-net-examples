using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Configuration (replace with real values)
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";
            string fromAddress = "sender@example.com";
            string toAddress = "recipient@example.com";
            string subject = "Task Request with Image Attachment";
            string body = "Please see the attached high‑resolution image.";
            string imagePath = "highres.jpg";
            const long maxAttachmentSize = 10L * 1024 * 1024; // 10 MB

            // Guard against placeholder credentials
            if (username == "username" || password == "password")
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping network operations.");
                return;
            }

            // Ensure the image file exists
            try
            {
                if (!File.Exists(imagePath))
                {
                    // Create a minimal placeholder image (a few bytes)
                    byte[] placeholder = new byte[] { 0xFF, 0xD8, 0xFF, 0xE0, 0x00, 0x10, 0x4A, 0x46, 0x49, 0x46, 0x00, 0x01 };
                    File.WriteAllBytes(imagePath, placeholder);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare attachment file: {ex.Message}");
                return;
            }

            // Verify attachment size limit
            long attachmentSize;
            try
            {
                attachmentSize = new FileInfo(imagePath).Length;
                if (attachmentSize > maxAttachmentSize)
                {
                    Console.Error.WriteLine($"Attachment size ({attachmentSize} bytes) exceeds the limit of {maxAttachmentSize} bytes.");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to evaluate attachment size: {ex.Message}");
                return;
            }

            // Create the mail message with attachment
            MailMessage message = new MailMessage();
            message.From = fromAddress;
            message.To.Add(toAddress);
            message.Subject = subject;
            message.Body = body;
            message.Attachments.Add(new Attachment(imagePath));

            // Send via EWS
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    client.Send(message);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"EWS operation failed: {ex.Message}");
                return;
            }

            Console.WriteLine("Task request with image attachment sent successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
