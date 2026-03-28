using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define paths
            string outputPath = Path.Combine(Environment.CurrentDirectory, "EmailWithAttachments.msg");
            string attachmentPath = Path.Combine(Environment.CurrentDirectory, "sample.txt");

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Ensure the attachment file exists; create a minimal placeholder if missing
            if (!File.Exists(attachmentPath))
            {
                File.WriteAllText(attachmentPath, "This is a placeholder attachment.");
            }

            // Create the email message
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@example.com";
                message.To = "recipient@example.com";
                message.Subject = "Message with Attachments";
                message.Body = "Please see the attached file.";

                // Add the attachment
                using (Attachment attachment = new Attachment(attachmentPath))
                {
                    message.Attachments.Add(attachment);
                }

                // Save the message as MSG
                message.Save(outputPath, SaveOptions.DefaultMsg);
            }

            Console.WriteLine("Message saved successfully to: " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
