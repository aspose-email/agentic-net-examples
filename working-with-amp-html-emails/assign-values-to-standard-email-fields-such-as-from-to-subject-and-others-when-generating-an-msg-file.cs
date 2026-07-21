using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mime;

class Program
{
    static void Main()
    {
        try
        {
            // Define output file path
            string outputPath = "output.msg";

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create a new mail message and assign standard fields
            using (MailMessage message = new MailMessage())
            {
                // From address
                message.From = new MailAddress("sender@example.com", "Sender Name");

                // To address
                message.To.Add(new MailAddress("recipient@example.com", "Recipient Name"));

                // CC address (optional)
                message.CC.Add(new MailAddress("cc@example.com", "CC Name"));

                // BCC address (optional)
                message.Bcc.Add(new MailAddress("bcc@example.com", "BCC Name"));

                // Subject
                message.Subject = "Sample MSG File Generated with Aspose.Email";

                // Body (plain text)
                message.Body = "Hello,\n\nThis is a sample email generated programmatically using Aspose.Email for .NET.\n\nBest regards,\nSender";

                // Add a simple attachment (optional)
                string attachmentPath = "sample.txt";
                if (File.Exists(attachmentPath))
                {
                    message.Attachments.Add(new Attachment(attachmentPath));
                }

                // Save the message as an MSG file
                message.Save(outputPath);
                Console.WriteLine($"Message saved successfully to '{outputPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
