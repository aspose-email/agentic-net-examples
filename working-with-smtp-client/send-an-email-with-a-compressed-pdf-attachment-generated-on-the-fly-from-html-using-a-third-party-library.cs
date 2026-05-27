using Aspose.Email.Clients;
using System;
using System.IO;
using System.IO.Compression;
using System.Text;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;
using Aspose.Words;

class Program
{
    static void Main()
    {
        try
        {
            // HTML content to be converted to PDF
            string htmlContent = "<html><body><h1>Hello World</h1><p>This is a PDF generated from HTML.</p></body></html>";

            // Convert HTML to PDF using Aspose.Words (in-memory)
            using (MemoryStream htmlStream = new MemoryStream(Encoding.UTF8.GetBytes(htmlContent)))
            using (MemoryStream pdfStream = new MemoryStream())
            {
                // Load HTML with explicit Aspose.Words.Loading.LoadOptions to avoid ambiguity
                Document doc = new Document(htmlStream, new Aspose.Words.Loading.LoadOptions
                {
                    LoadFormat = Aspose.Words.LoadFormat.Html
                });

                // Save PDF to memory stream
                doc.Save(pdfStream, Aspose.Words.SaveFormat.Pdf);
                pdfStream.Position = 0;

                // Compress the PDF into a ZIP archive (in-memory)
                using (MemoryStream zipStream = new MemoryStream())
                {
                    using (ZipArchive zip = new ZipArchive(zipStream, ZipArchiveMode.Create, true))
                    {
                        ZipArchiveEntry entry = zip.CreateEntry("document.pdf", CompressionLevel.Optimal);
                        using (Stream entryStream = entry.Open())
                        {
                            pdfStream.CopyTo(entryStream);
                        }
                    }
                    zipStream.Position = 0;

                    // Prepare the email message
                    using (MailMessage mail = new MailMessage())
                    {
                        mail.From = "sender@example.com";
                        mail.To = "recipient@example.com";
                        mail.Subject = "Email with PDF attachment (zipped)";
                        mail.Body = "Please find the attached PDF (compressed as ZIP).";
                        mail.IsBodyHtml = false;

                        // Attach the ZIP archive
                        mail.Attachments.Add(new Attachment(zipStream, "document.zip", "application/zip"));

                        // SMTP client configuration (placeholder values)
                        string smtpHost = "smtp.example.com";
                        int smtpPort = 587;
                        string smtpUser = "username";
                        string smtpPass = "password";

                        // Guard against placeholder credentials
                        bool isPlaceholder = smtpHost.Contains("example.com") ||
                                             smtpUser.Equals("username", StringComparison.OrdinalIgnoreCase) ||
                                             smtpPass.Equals("password", StringComparison.OrdinalIgnoreCase);

                        if (isPlaceholder)
                        {
                            Console.Error.WriteLine("Placeholder SMTP credentials detected. Skipping email send.");
                            return;
                        }

                        // Send the email using Aspose.Email SmtpClient
                        try
                        {
                            using (SmtpClient client = new SmtpClient(smtpHost, smtpPort, smtpUser, smtpPass, SecurityOptions.Auto))
                            {
                                client.Send(mail);
                                Console.WriteLine("Email sent successfully.");
                            }
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
