using System;
using System.IO;
using Aspose.Email;

namespace EmailAttachmentExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string attachmentPath = "sample.txt";
                if (!File.Exists(attachmentPath))
                {
                    // Create a minimal placeholder file if it does not exist
                    File.WriteAllText(attachmentPath, "Placeholder content");
                }

                using (MailMessage message = new MailMessage())
                {
                    message.From = "sender@example.com";
                    message.To.Add("receiver@example.com");
                    message.Subject = "Test email with attachment";
                    message.Body = "Please see the attached file.";

                    using (FileStream fileStream = new FileStream(attachmentPath, FileMode.Open, FileAccess.Read))
                    {
                        using (Attachment attachment = new Attachment(fileStream, Path.GetFileName(attachmentPath)))
                        {
                            message.Attachments.Add(attachment);

                            // Save the email to a file for demonstration purposes
                            string emlPath = "EmailWithAttachment.eml";
                            message.Save(emlPath);
                            Console.WriteLine($"Email saved to {emlPath}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
