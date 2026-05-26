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
            string host = "smtp.example.com";
            int port = 587;
            string username = "user@example.com";
            string password = "password";

            // Email details
            string from = "sender@example.com";
            string to = "recipient@example.com";
            string subject = "Test email with large attachment";
            string body = "Please see the attached file.";
            string attachmentPath = "largefile.bin";

            // Skip execution when placeholder credentials are detected
            if (host.Contains("example.com") || username.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP settings detected. Skipping send operation.");
                return;
            }

            // Ensure the attachment file exists; create a minimal placeholder if missing
            if (!File.Exists(attachmentPath))
            {
                try
                {
                    using (FileStream fs = File.Create(attachmentPath))
                    {
                        byte[] placeholder = new byte[1024]; // 1 KB placeholder content
                        fs.Write(placeholder, 0, placeholder.Length);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder attachment: {ex.Message}");
                    return;
                }
            }

            // Create the email message
            using (MailMessage message = new MailMessage())
            {
                message.From = from;
                message.To.Add(to);
                message.Subject = subject;
                message.Body = body;

                // Add the attachment
                try
                {
                    Attachment attachment = new Attachment(attachmentPath);
                    message.Attachments.Add(attachment);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to add attachment: {ex.Message}");
                    return;
                }

                // Create the SMTP client and set a custom timeout of two minutes (120,000 ms)
                using (SmtpClient client = new SmtpClient(host, port, username, password))
                {
                    client.Timeout = 120000; // 2 minutes

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
