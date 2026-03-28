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
            // Prepare output directory
            string outputDirectory = Path.GetFullPath("Output");
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            string outputPath = Path.Combine(outputDirectory, "SerializedMessage.msg");

            // Prepare a sample attachment file
            string attachmentPath = Path.Combine(outputDirectory, "sample.txt");
            if (!File.Exists(attachmentPath))
            {
                try
                {
                    File.WriteAllText(attachmentPath, "This is a sample attachment.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating attachment file: {ex.Message}");
                    return;
                }
            }

            // Create and configure the mail message
            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress("sender@example.com", "Sender Name");
                message.To.Add(new MailAddress("recipient@example.com", "Recipient Name"));
                message.Subject = "Sample MSG Serialization";
                message.Body = "This email demonstrates serialization to MSG format with all headers and attachments preserved.";

                // Add a custom header
                message.Headers.Add("X-Custom-Header", "CustomHeaderValue");

                // Add the attachment
                using (Attachment attachment = new Attachment(attachmentPath))
                {
                    message.Attachments.Add(attachment);
                }

                // Configure MSG save options
                MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat)
                {
                    PreserveOriginalDates = true
                };

                // Save the message as MSG
                try
                {
                    message.Save(outputPath, saveOptions);
                    Console.WriteLine($"Message successfully saved to: {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error saving MSG file: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
