using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace EmailMsgPersistence
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define output MSG file path
                string outputPath = "output.msg";

                // Ensure the output directory exists
                string outputDir = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }

                // Create a new email message
                MailMessage message = new MailMessage();
                message.From = new MailAddress("sender@example.com", "Sender Name");
                message.To.Add(new MailAddress("recipient@example.com", "Recipient Name"));
                message.Subject = "Sample Email with Attachments";
                message.HtmlBody = "<h1>Hello World</h1><p>This is a sample email.</p>";

                // Add a custom header
                message.Headers.Add("X-Custom-Header", "CustomValue");

                // Add an attachment (creates a simple text file in memory)
                byte[] attachmentData = System.Text.Encoding.UTF8.GetBytes("This is the attachment content.");
                using (MemoryStream attachmentStream = new MemoryStream(attachmentData))
                {
                    Attachment attachment = new Attachment(attachmentStream, "sample.txt", "text/plain");
                    message.Attachments.Add(attachment);

                    // Prepare MSG save options to preserve original dates
                    MsgSaveOptions msgSaveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode)
                    {
                        PreserveOriginalDates = true
                    };

                    // Save the message as MSG
                    message.Save(outputPath, msgSaveOptions);
                }

                Console.WriteLine($"Message successfully saved to '{outputPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                // Graceful exit without rethrowing
            }
        }
    }
}
