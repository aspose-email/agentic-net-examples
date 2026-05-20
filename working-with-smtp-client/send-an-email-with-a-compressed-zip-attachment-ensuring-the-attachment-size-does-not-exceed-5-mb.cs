using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // SMTP server configuration (replace with real values)
            string smtpHost = "smtp.example.com";
            int smtpPort = 587;
            string smtpUsername = "user@example.com";
            string smtpPassword = "password";

            // Guard against placeholder credentials/host
            if (smtpHost.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP host detected. Skipping send operation.");
                return;
            }

            // Path to the ZIP attachment
            string attachmentPath = "attachment.zip";

            // Verify attachment file exists and size limit (5 MB)
            if (!File.Exists(attachmentPath))
            {
                Console.Error.WriteLine($"Attachment file not found: {attachmentPath}");
                return;
            }

            FileInfo attachmentInfo;
            try
            {
                attachmentInfo = new FileInfo(attachmentPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to access attachment file: {ex.Message}");
                return;
            }

            const long maxAttachmentSize = 5L * 1024 * 1024; // 5 MB
            if (attachmentInfo.Length > maxAttachmentSize)
            {
                Console.Error.WriteLine("Attachment exceeds the 5 MB size limit.");
                return;
            }

            // Create the email message
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Test email with ZIP attachment";
                message.Body = "Please find the attached ZIP file.";

                // Add the attachment
                using (Attachment attachment = new Attachment(attachmentPath))
                {
                    message.Attachments.Add(attachment);

                    // Send the email via SMTP
                    using (SmtpClient client = new SmtpClient(smtpHost, smtpPort, smtpUsername, smtpPassword))
                    {
                        try
                        {
                            client.Send(message);
                            Console.WriteLine("Email sent successfully.");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to send email: {ex.Message}");
                        }
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
