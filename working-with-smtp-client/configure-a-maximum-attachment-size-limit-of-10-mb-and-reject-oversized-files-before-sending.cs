using System;
using System.IO;
using System.Collections.Generic;
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
            string username = "user@example.com";
            string password = "password";

            // Guard against placeholder credentials/host
            if (smtpHost.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP host detected. Skipping send operation.");
                return;
            }

            // Create the email message
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Test Email with Attachment Size Check";
                message.Body = "Please see attached files.";

                // Define attachment file paths
                List<string> attachmentPaths = new List<string>
                {
                    "C:\\temp\\file1.pdf",
                    "C:\\temp\\file2.jpg"
                };

                const long maxSizeBytes = 10L * 1024 * 1024; // 10 MB
                long totalAttachmentSize = 0;

                foreach (string path in attachmentPaths)
                {
                    if (!File.Exists(path))
                    {
                        Console.Error.WriteLine($"Attachment file not found: {path}");
                        continue; // Skip missing files
                    }

                    try
                    {
                        FileInfo info = new FileInfo(path);
                        totalAttachmentSize += info.Length;

                        if (totalAttachmentSize > maxSizeBytes)
                        {
                            Console.Error.WriteLine("Total attachment size exceeds 10 MB limit. Email will not be sent.");
                            return;
                        }

                        using (Attachment attachment = new Attachment(path))
                        {
                            message.Attachments.Add(attachment);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error processing attachment '{path}': {ex.Message}");
                        return;
                    }
                }

                // Send the email
                using (SmtpClient client = new SmtpClient(smtpHost, smtpPort, username, password))
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
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
