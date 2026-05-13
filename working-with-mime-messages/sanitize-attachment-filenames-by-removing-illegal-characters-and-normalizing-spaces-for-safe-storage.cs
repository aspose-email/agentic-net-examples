using System;
using System.IO;
using System.Text.RegularExpressions;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Input EML file path
            string inputPath = "input.eml";

            // Ensure the input file exists; create a minimal placeholder if missing
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
                    using (MailMessage placeholder = new MailMessage())
                    {
                        placeholder.From = "placeholder@example.com";
                        placeholder.To = "recipient@example.com";
                        placeholder.Subject = "Placeholder Email";
                        placeholder.Body = "This is a placeholder email generated because the input file was missing.";
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder email: {ex.Message}");
                    return;
                }
            }

            // Output directory for sanitized attachments
            string outputDir = "SanitizedAttachments";

            // Ensure the output directory exists
            try
            {
                if (!Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory '{outputDir}': {ex.Message}");
                return;
            }

            // Load the email message
            using (MailMessage mailMessage = MailMessage.Load(inputPath))
            {
                // Process each attachment
                foreach (Attachment attachment in mailMessage.Attachments)
                {
                    string originalName = attachment.Name ?? "unnamed";
                    string sanitizedName = SanitizeFileName(originalName);

                    // If sanitization results in an empty name, generate a fallback name
                    if (string.IsNullOrEmpty(sanitizedName))
                    {
                        sanitizedName = Guid.NewGuid().ToString() + ".dat";
                    }

                    string outputPath = Path.Combine(outputDir, sanitizedName);

                    // Save the attachment safely
                    try
                    {
                        attachment.Save(outputPath);
                        Console.WriteLine($"Saved attachment '{originalName}' as '{sanitizedName}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save attachment '{originalName}': {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Removes illegal filename characters and normalizes spaces
    private static string SanitizeFileName(string fileName)
    {
        // Remove invalid file name characters
        char[] invalidChars = Path.GetInvalidFileNameChars();
        foreach (char invalidChar in invalidChars)
        {
            fileName = fileName.Replace(invalidChar.ToString(), string.Empty);
        }

        // Replace multiple whitespace with a single space and trim
        fileName = Regex.Replace(fileName, @"\s+", " ").Trim();

        return fileName;
    }
}
