using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.eml";
            string outputPath = "output.eml";
            string attachmentNameToRemove = "remove.txt";

            // Ensure input file exists; create a minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                try
                {
                    MailMessage placeholder = new MailMessage
                    {
                        From = "sender@example.com",
                        To = "receiver@example.com",
                        Subject = "Placeholder",
                        Body = "This is a placeholder email."
                    };
                    Attachment placeholderAttachment = new Attachment("remove.txt");
                    placeholder.Attachments.Add(placeholderAttachment);
                    placeholder.Save(inputPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder input file: {ex.Message}");
                    return;
                }
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Load the email message
            MailMessage message;
            try
            {
                message = MailMessage.Load(inputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load email from '{inputPath}': {ex.Message}");
                return;
            }

            using (message)
            {
                // Locate the attachment to remove
                Attachment attachmentToRemove = null;
                foreach (Attachment att in message.Attachments)
                {
                    if (string.Equals(att.Name, attachmentNameToRemove, StringComparison.OrdinalIgnoreCase))
                    {
                        attachmentToRemove = att;
                        break;
                    }
                }

                if (attachmentToRemove != null)
                {
                    // Remove the attachment
                    message.Attachments.Remove(attachmentToRemove);
                    Console.WriteLine($"Attachment '{attachmentNameToRemove}' removed.");
                }
                else
                {
                    Console.WriteLine($"Attachment '{attachmentNameToRemove}' not found.");
                }

                // Save the modified message
                try
                {
                    message.Save(outputPath);
                    Console.WriteLine($"Modified email saved to '{outputPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save email to '{outputPath}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
