using System;
using System.IO;
using System.Text;
using Aspose.Email;
using Aspose.Email.Clients.Smtp;
using Aspose.Words;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Placeholder SMTP configuration – skip actual send in CI environments
            string smtpHost = "smtp.example.com";
            int smtpPort = 587;
            string smtpUser = "user@example.com";
            string smtpPass = "password";

            if (smtpHost.Contains("example.com"))
            {
                Console.Error.WriteLine("Placeholder SMTP configuration detected. Skipping email send.");
                return;
            }

            // HTML content to be converted into PDF
            string htmlContent = "<html><body><h1>Hello PDF</h1><p>This PDF is generated from HTML at runtime.</p></body></html>";

            // Convert HTML to PDF using Aspose.Words
            using (MemoryStream htmlStream = new MemoryStream(Encoding.UTF8.GetBytes(htmlContent)))
            {
                var loadOptions = new Aspose.Words.Loading.LoadOptions { LoadFormat = Aspose.Words.LoadFormat.Html };
                var doc = new Document(htmlStream, loadOptions);
                using (MemoryStream pdfStream = new MemoryStream())
                {
                    doc.Save(pdfStream, Aspose.Words.SaveFormat.Pdf);
                    pdfStream.Position = 0;

                    // Create the email message
                    using (MailMessage message = new MailMessage())
                    {
                        message.From = "sender@example.com";
                        message.To.Add("recipient@example.com");
                        message.Subject = "Test Email with PDF Attachment";
                        message.Body = "Please find the generated PDF attached.";

                        // Attach the PDF
                        var attachment = new Attachment(pdfStream, "Generated.pdf", "application/pdf");
                        message.Attachments.Add(attachment);

                        // Send the email via SMTP
                        using (SmtpClient client = new SmtpClient(smtpHost, smtpPort, smtpUser, smtpPass))
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
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
