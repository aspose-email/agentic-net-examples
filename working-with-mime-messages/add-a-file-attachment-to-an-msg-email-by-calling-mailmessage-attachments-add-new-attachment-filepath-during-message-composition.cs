using System;
using System.IO;
using Aspose.Email;

namespace AddAttachmentExample
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
                    Console.Error.WriteLine($"Error: File not found – {attachmentPath}");
                    return;
                }

                using (MailMessage message = new MailMessage())
                {
                    message.From = "sender@example.com";
                    message.To = "receiver@example.com";
                    message.Subject = "Message with attachment";
                    message.Body = "Please see the attached file.";

                    using (Attachment attachment = new Attachment(attachmentPath))
                    {
                        message.Attachments.Add(attachment);
                    }

                    string outputPath = "MessageWithAttachment.eml";
                    try
                    {
                        message.Save(outputPath);
                        Console.WriteLine($"Message saved to {outputPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error saving message: {ex.Message}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
