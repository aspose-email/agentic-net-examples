using System;
using System.IO;
using System.Text.RegularExpressions;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input and output folder paths (can be passed as command‑line arguments)
            string inputFolderPath = args.Length > 0 ? args[0] : "InputEml";
            string outputFolderPath = args.Length > 1 ? args[1] : "CleanedEml";

            // Verify input folder exists
            if (!Directory.Exists(inputFolderPath))
            {
                Console.Error.WriteLine($"Input folder does not exist: {inputFolderPath}");
                return;
            }

            // Ensure output folder exists
            try
            {
                if (!Directory.Exists(outputFolderPath))
                {
                    Directory.CreateDirectory(outputFolderPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output folder '{outputFolderPath}': {ex.Message}");
                return;
            }

            // Prepare regex to remove tracking pixel <img> tags
            // Removes <img> tags with width or height of 1 pixel or src containing "tracking"
            string pattern = @"<img\b[^>]*?(?:width\s*=\s*[""']?1[""']?|height\s*=\s*[""']?1[""']?|src\s*=\s*[""'][^""']*tracking[^""']*[""'])[^>]*?>";
            Regex trackingPixelRegex = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);

            // Process each .eml file in the input folder
            string[] emlFiles = Directory.GetFiles(inputFolderPath, "*.eml");
            foreach (string emlFilePath in emlFiles)
            {
                // Guard against missing file (should not happen after GetFiles, but defensive)
                if (!File.Exists(emlFilePath))
                {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(emlFilePath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"File not found, skipping: {emlFilePath}");
                    continue;
                }

                try
                {
                    // Load the email message
                    using (MailMessage mailMessage = MailMessage.Load(emlFilePath))
                    {
                        // Remove tracking pixels from HTML body if present
                        if (!string.IsNullOrEmpty(mailMessage.HtmlBody))
                        {
                            string cleanedHtml = trackingPixelRegex.Replace(mailMessage.HtmlBody, string.Empty);
                            mailMessage.HtmlBody = cleanedHtml;
                        }

                        // Determine output file path
                        string outputFilePath = Path.Combine(outputFolderPath, Path.GetFileName(emlFilePath));

                        // Save the cleaned message
                        mailMessage.Save(outputFilePath);
                        Console.WriteLine($"Processed and saved: {outputFilePath}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing file '{emlFilePath}': {ex.Message}");
                    // Continue with next file
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
