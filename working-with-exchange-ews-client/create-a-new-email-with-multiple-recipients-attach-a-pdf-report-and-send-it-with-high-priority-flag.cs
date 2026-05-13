using System;
using System.IO;
using System.Net;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – skip network call if they are not real.
            string mailboxUri = "https://example.com/EWS/Exchange.asmx";
            string username = "username";
            string password = "password";

            if (mailboxUri.Contains("example") || username == "username" || password == "password")
            {
                Console.WriteLine("Placeholder credentials detected. Skipping send operation.");
                return;
            }

            // Ensure the PDF attachment exists.
            string attachmentPath = "report.pdf";
            if (!File.Exists(attachmentPath))
            {
                try
                {
                    using (FileStream fs = File.Create(attachmentPath))
                    {
                        // Minimal PDF header to make a valid (though empty) PDF file.
                        byte[] pdfHeader = System.Text.Encoding.ASCII.GetBytes("%PDF-1.4\n%âãÏÓ\n");
                        fs.Write(pdfHeader, 0, pdfHeader.Length);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder attachment: {ex.Message}");
                    return;
                }
            }

            // Create the email message.
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("sender@example.com");
            mail.To.Add(new MailAddress("recipient1@example.com"));
            mail.To.Add(new MailAddress("recipient2@example.com"));
            mail.CC.Add(new MailAddress("cc@example.com"));
            mail.Bcc.Add(new MailAddress("bcc@example.com"));
            mail.Subject = "Monthly Report";
            mail.Body = "Please find the attached report.";
            mail.Priority = MailPriority.High;
            mail.Attachments.Add(new Attachment(attachmentPath));

            // Send the message using EWS.
            using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
            {
                try
                {
                    client.Send(mail);
                    Console.WriteLine("Email sent successfully.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to send email: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
