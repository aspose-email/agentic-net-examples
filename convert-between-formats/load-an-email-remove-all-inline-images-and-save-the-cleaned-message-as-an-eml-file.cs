using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define source and target file paths
            string sourcePath = "source.eml";
            string targetPath = "cleaned.eml";

            // Verify source file exists
            if (!File.Exists(sourcePath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(sourcePath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Source file not found: {sourcePath}");
                return;
            }

            // Load the email message
            using (MailMessage mailMessage = MailMessage.Load(sourcePath))
            {
                // Remove all inline attachments (identified by a non‑empty ContentId)
                for (int i = mailMessage.Attachments.Count - 1; i >= 0; i--)
                {
                    Attachment attachment = mailMessage.Attachments[i];
                    if (!string.IsNullOrEmpty(attachment.ContentId))
                    {
                        mailMessage.Attachments.RemoveAt(i);
                    }
                }

                // Save the cleaned message as EML
                mailMessage.Save(targetPath);
                Console.WriteLine($"Cleaned email saved to: {targetPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
