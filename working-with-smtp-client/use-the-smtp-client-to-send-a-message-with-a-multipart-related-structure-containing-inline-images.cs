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

            // Detect placeholder configuration and skip actual send
            if (smtpHost.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping send operation.");
                return;
            }

            // Prepare inline image
            string imagePath = "inline.png";
            if (!File.Exists(imagePath))
            {
                try
                {
                    // Create a minimal 1x1 PNG placeholder
                    byte[] pngBytes = new byte[]
                    {
                        0x89,0x50,0x4E,0x47,0x0D,0x0A,0x1A,0x0A,
                        0x00,0x00,0x00,0x0D,0x49,0x48,0x44,0x52,
                        0x00,0x00,0x00,0x01,0x00,0x00,0x00,0x01,
                        0x08,0x06,0x00,0x00,0x00,0x1F,0x15,0xC4,
                        0x89,0x00,0x00,0x00,0x0A,0x49,0x44,0x41,
                        0x54,0x78,0x9C,0x63,0x60,0x00,0x00,0x00,
                        0x02,0x00,0x01,0xE2,0x21,0xBC,0x33,0x00,
                        0x00,0x00,0x00,0x49,0x45,0x4E,0x44,0xAE,
                        0x42,0x60,0x82
                    };
                    File.WriteAllBytes(imagePath, pngBytes);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder image: {ex.Message}");
                    return;
                }
            }

            // Build the email message with inline image
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To.Add("recipient@example.com");
                message.Subject = "Email with Inline Image";

                // HTML body referencing the inline image via Content-ID
                message.IsBodyHtml = true;
                message.HtmlBody = @"<html><body><h1>Hello</h1><p>Here is an inline image:</p><img src=""cid:image1""/></body></html>";

                // Add the image as a linked resource
                try
                {
                    using (FileStream imgStream = File.OpenRead(imagePath))
                    {
                        var linkedResource = new LinkedResource(imgStream)
                        {
                            ContentId = "image1"
                        };
                        message.LinkedResources.Add(linkedResource);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to attach inline image: {ex.Message}");
                    return;
                }

                // Send the message via SMTP
                try
                {
                    using (SmtpClient client = new SmtpClient(smtpHost, smtpPort))
                    {
                        client.Username = smtpUser;
                        client.Password = smtpPass;
                        client.SecurityOptions = SecurityOptions.Auto;
                        client.Send(message);
                        Console.WriteLine("Message sent successfully.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"SMTP send failed: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
