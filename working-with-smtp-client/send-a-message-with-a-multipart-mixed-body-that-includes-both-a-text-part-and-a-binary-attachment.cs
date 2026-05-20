using Aspose.Email.Clients;
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
            // Placeholder SMTP configuration
            string smtpHost = "smtp.example.com";
            int smtpPort = 587;
            string smtpUser = "user@example.com";
            string smtpPass = "password";

            // Skip actual sending when using placeholder credentials
            if (smtpHost.Contains("example.com"))
            {
                Console.WriteLine("Placeholder SMTP settings detected. Skipping send operation.");
                return;
            }

            // Ensure attachment file exists
            string attachmentPath = "sample.bin";
            if (!File.Exists(attachmentPath))
            {
                try
                {
                    File.WriteAllBytes(attachmentPath, new byte[] { 0x01, 0x02, 0x03, 0x04 });
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create attachment file: {ex.Message}");
                    return;
                }
            }

            // Create the email message with a plain text body
            using (MailMessage message = new MailMessage("sender@example.com", "recipient@example.com", "Multipart/Mixed Email", "This is the plain text part of the email."))
            {
                // Add binary attachment
                try
                {
                    message.Attachments.Add(new Attachment(attachmentPath));
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to add attachment: {ex.Message}");
                    return;
                }

                // Send the message via SMTP
                try
                {
                    using (SmtpClient client = new SmtpClient(smtpHost, smtpPort, smtpUser, smtpPass))
                    {
                        client.SecurityOptions = SecurityOptions.Auto;
                        client.Send(message);
                        Console.WriteLine("Message sent successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to send message: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
