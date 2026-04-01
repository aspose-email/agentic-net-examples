using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define paths
            string attachmentPath = "sample.txt";
            string outputMsgPath = "output.msg";

            // Ensure attachment file exists; create a minimal placeholder if missing
            if (!File.Exists(attachmentPath))
            {
                try
                {
                    File.WriteAllText(attachmentPath, "Placeholder attachment content.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder attachment: {ex.Message}");
                    return;
                }
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputMsgPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Create the email message
            using (MailMessage mailMessage = new MailMessage())
            {
                // Set basic properties
                mailMessage.From = new MailAddress("sender@example.com", "Sender Name");
                mailMessage.To.Add(new MailAddress("recipient@example.com", "Recipient Name"));
                mailMessage.Subject = "Sample Email with Attachment";
                mailMessage.Body = "This is the plain text body of the email.";

                // Add a custom header
                mailMessage.Headers["X-Custom-Header"] = "CustomHeaderValue";

                // Add attachment
                using (Attachment attachment = new Attachment(attachmentPath))
                {
                    // The Name property is used for the attachment filename
                    attachment.Name = Path.GetFileName(attachmentPath);
                    mailMessage.Attachments.Add(attachment);
                }

                // Display all headers using the required iteration pattern
                foreach (string key in mailMessage.Headers.Keys)
                {
                    Console.WriteLine($"{key}: {mailMessage.Headers[key]}");
                }

                // Save as MSG using MsgSaveOptions
                MsgSaveOptions saveOptions = new MsgSaveOptions(MailMessageSaveType.OutlookMessageFormat);
                try
                {
                    mailMessage.Save(outputMsgPath, saveOptions);
                    Console.WriteLine($"Message saved successfully to '{outputMsgPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save MSG file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
