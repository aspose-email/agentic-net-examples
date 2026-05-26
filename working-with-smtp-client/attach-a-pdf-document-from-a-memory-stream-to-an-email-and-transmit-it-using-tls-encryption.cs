using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;
using Aspose.Email.Clients;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder SMTP settings – skip actual send if they are not real.
            string smtpHost = "smtp.example.com";
            int smtpPort = 587;
            string smtpUser = "user@example.com";
            string smtpPass = "password";

            if (smtpHost.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping email transmission.");
                return;
            }

            // Create a simple PDF content in memory (placeholder bytes).
            byte[] pdfBytes = new byte[] { 0x25, 0x50, 0x44, 0x46, 0x2D }; // "%PDF-"
            using (MemoryStream pdfStream = new MemoryStream(pdfBytes))
            {
                // Build the email message.
                using (MailMessage message = new MailMessage())
                {
                    message.From = "sender@example.com";
                    message.To.Add("recipient@example.com");
                    message.Subject = "Test Email with PDF Attachment";
                    message.Body = "Please find the PDF attached.";

                    // Attach the PDF from the memory stream.
                    Attachment pdfAttachment = new Attachment(pdfStream, "application/pdf")
                    {
                        Name = "sample.pdf"
                    };
                    message.Attachments.Add(pdfAttachment);

                    // Send the message using TLS encryption.
                    using (SmtpClient client = new SmtpClient())
                    {
                        client.Host = smtpHost;
                        client.Port = smtpPort;
                        client.Username = smtpUser;
                        client.Password = smtpPass;
                        client.SecurityOptions = SecurityOptions.Auto; // Enables TLS/SSL as appropriate.

                        try
                        {
                            client.Send(message);
                            Console.WriteLine("Email sent successfully.");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to send email: {ex.Message}");
                            return;
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
