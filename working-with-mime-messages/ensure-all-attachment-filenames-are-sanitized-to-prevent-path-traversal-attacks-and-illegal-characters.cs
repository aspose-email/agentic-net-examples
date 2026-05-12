using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            const string inputPath = "input.eml";
            const string outputDir = "output";

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

                using (var placeholder = new MailMessage("sender@example.com", "receiver@example.com", "Placeholder", "This is a placeholder email."))
                {
                    placeholder.Save(inputPath);
                }
            }

            // Ensure output directory exists
            Directory.CreateDirectory(outputDir);

            // Load the email message
            using (MailMessage message = MailMessage.Load(inputPath))
            {
                foreach (Attachment attachment in message.Attachments)
                {
                    // Sanitize attachment name
                    string originalName = attachment.Name ?? "attachment";
                    string safeName = SanitizeFileName(originalName);
                    string savePath = Path.Combine(outputDir, safeName);

                    // Save the attachment safely
                    using (attachment)
                    {
                        attachment.Save(savePath);
                    }

                    Console.WriteLine($"Saved attachment: {savePath}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Removes invalid characters and path‑traversal sequences from a file name
    static string SanitizeFileName(string fileName)
    {
        foreach (char invalid in Path.GetInvalidFileNameChars())
        {
            fileName = fileName.Replace(invalid.ToString(), "_");
        }

        // Replace directory separators
        fileName = fileName.Replace(Path.DirectorySeparatorChar.ToString(), "_")
                           .Replace(Path.AltDirectorySeparatorChar.ToString(), "_");

        // Collapse any ".." sequences
        while (fileName.Contains(".."))
        {
            fileName = fileName.Replace("..", "_");
        }

        // Trim whitespace
        return fileName.Trim();
    }
}
