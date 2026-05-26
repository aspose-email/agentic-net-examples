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
            // Prepare a simple email message
            MailMessage originalMessage = new MailMessage(
                "sender@example.com",
                "recipient@example.com",
                "Compressed MIME Example",
                "This is the body of the email."
            );

            // Save the message as MIME (EML) into a memory stream
            using (MemoryStream mimeStream = new MemoryStream())
            {
                originalMessage.Save(mimeStream);
                mimeStream.Position = 0;

                // Compress the MIME stream using GZIP
                using (MemoryStream compressedStream = new MemoryStream())
                {
                    using (GZipStream gzip = new GZipStream(compressedStream, CompressionMode.Compress, true))
                    {
                        mimeStream.CopyTo(gzip);
                    }

                    // Prepare the compressed data as an attachment
                    compressedStream.Position = 0;
                    Attachment gzipAttachment = new Attachment(compressedStream, "message.eml.gz", "application/gzip");

                    // Create a new message that will be sent via SMTP
                    MailMessage sendMessage = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Compressed MIME Email",
                        "The original MIME message is attached in GZIP format."
                    );
                    sendMessage.Attachments.Add(gzipAttachment);

                    // SMTP client configuration (placeholder values)
                    string smtpHost = "smtp.example.com";
                    int smtpPort = 587;
                    string username = "user@example.com";
                    string password = "password";

                    // Guard against placeholder credentials/hosts
                    if (smtpHost.Contains("example.com"))
                    {
                        Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping send operation.");
                        return;
                    }

                    // Send the email using SMTP
                    using (SmtpClient client = new SmtpClient(smtpHost, smtpPort, username, password))
                    {
                        try
                        {
                            client.Send(sendMessage);
                            Console.WriteLine("Email sent successfully.");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"SMTP send error: {ex.Message}");
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
