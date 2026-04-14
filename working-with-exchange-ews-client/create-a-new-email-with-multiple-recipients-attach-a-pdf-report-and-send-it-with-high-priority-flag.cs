using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Placeholder credentials – replace with real values or skip execution.
            string mailboxUri = "https://your.exchange.server/EWS/Exchange.asmx";
            string username = "your_username";
            string password = "your_password";

            // Simple guard to avoid external calls when placeholders are used.
            if (string.IsNullOrWhiteSpace(mailboxUri) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) ||
                mailboxUri.Contains("your.") ||
                username.Contains("your_") ||
                password.Contains("your_"))
            {
                Console.Error.WriteLine("Placeholder credentials detected. Skipping send operation.");
                return;
            }

            // Path to the PDF report to attach.
            string pdfPath = "Report.pdf";

            // Ensure the PDF file exists; create a minimal placeholder if missing.
            try
            {
                if (!File.Exists(pdfPath))
                {
                    // Minimal PDF header (just enough to be a valid PDF file).
                    byte[] minimalPdf = System.Text.Encoding.ASCII.GetBytes("%PDF-1.1\n%âãÏÓ\n1 0 obj\n<<>>\nendobj\ntrailer\n<<>>\n%%EOF");
                    File.WriteAllBytes(pdfPath, minimalPdf);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare attachment file: {ex.Message}");
                return;
            }

            // Create the email message.
            MailMessage message = new MailMessage();
            message.From = new MailAddress("sender@example.com");
            message.To.Add("recipient1@example.com");
            message.To.Add("recipient2@example.com");
            message.CC.Add("ccrecipient@example.com");
            message.Bcc.Add("bccrecipient@example.com");
            message.Subject = "Monthly Report";
            message.Body = "Please find the attached PDF report.";
            message.Priority = MailPriority.High;

            // Attach the PDF report.
            try
            {
                Attachment attachment = new Attachment(pdfPath);
                message.Attachments.Add(attachment);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to attach PDF: {ex.Message}");
                return;
            }

            // Send the message using EWS.
            try
            {
                using (IEWSClient client = EWSClient.GetEWSClient(mailboxUri, username, password))
                {
                    client.Send(message);
                }

                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to send email via EWS: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
