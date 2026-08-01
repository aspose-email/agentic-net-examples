using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Output MSG file path
            const string outputMsgPath = "AddAttachments.msg";

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputMsgPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create the email message
            using (MailMessage message = new MailMessage())
            {
                message.From = "sender@from.com";
                message.To = "receiver@to.com";
                message.Subject = "Message with attachments";
                message.Body = "Please see the attached files.";

                // List of files to attach
                string[] attachmentFiles = new string[]
                {
                    "1.txt",
                    "1.jpg",
                    "1.doc",
                    "1.rar",
                    "1.pdf"
                };

                foreach (string filePath in attachmentFiles)
                {
                    // Guard file existence; create a minimal placeholder if missing
                    if (!File.Exists(filePath))
                    {
                        try
                        {
                            File.WriteAllText(filePath, $"Placeholder content for {Path.GetFileName(filePath)}");
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to create placeholder for '{filePath}': {ex.Message}");
                            continue;
                        }
                    }

                    // Add the attachment to the message
                    try
                    {
                        Attachment attachment = new Attachment(filePath);
                        message.AddAttachment(attachment);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to add attachment '{filePath}': {ex.Message}");
                    }
                }

                // Save the message as MSG with proper MIME encoding
                try
                {
                    message.Save(outputMsgPath, SaveOptions.DefaultMsg);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save MSG file '{outputMsgPath}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
