using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients;
using Aspose.Email.Clients.Smtp;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for attachments
            string pdfPath = "sample.pdf";
            string imgPath = "image.jpg";

            // Ensure PDF exists (create minimal placeholder if missing)
            if (!File.Exists(pdfPath))
            {
                try
                {
                    // Minimal PDF header
                    File.WriteAllBytes(pdfPath, new byte[] { 0x25, 0x50, 0x44, 0x46, 0x0A });
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PDF: {ex.Message}");
                    return;
                }
            }

            // Ensure image exists (create minimal JPEG placeholder if missing)
            if (!File.Exists(imgPath))
            {
                try
                {
                    // Minimal JPEG header
                    File.WriteAllBytes(imgPath, new byte[] { 0xFF, 0xD8, 0xFF, 0xE0, 0x00, 0x10, 0x4A, 0x46, 0x49, 0x46, 0x00, 0x01 });
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder image: {ex.Message}");
                    return;
                }
            }

            // Gmail SMTP credentials (replace with real values)
            string username = "your.email@gmail.com";
            string password = "yourpassword";

            // Skip actual send when placeholder credentials are detected
            if (username.Contains("example.com") || password == "yourpassword")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping email send.");
                return;
            }

            // Create and configure SMTP client (STARTTLS)
            using (SmtpClient client = new SmtpClient("smtp.gmail.com", 587, username, password, SecurityOptions.SSLExplicit))
            {
                try
                {
                    client.ValidateCredentials();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"SMTP connection/validation failed: {ex.Message}");
                    return;
                }

                // Build the email message
                using (MailMessage message = new MailMessage())
                {
                    message.From = username;
                    message.To.Add(username); // send to self for demonstration
                    message.Subject = "Test email with PDF and image attachments";
                    message.Body = "Please find the attached PDF and image files.";

                    // Add attachments
                    try
                    {
                        message.Attachments.Add(new Attachment(pdfPath));
                        message.Attachments.Add(new Attachment(imgPath));
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to add attachments: {ex.Message}");
                        return;
                    }

                    // Send the email
                    try
                    {
                        client.Send(message);
                        Console.WriteLine("Email sent successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Send operation failed: {ex.Message}");
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
