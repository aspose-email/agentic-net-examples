using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "HtmlEmail.eml";
            string outputPath = "PlainText.txt";

            // Verify input file exists
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

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }
            }

            // Load the email message
            using (MailMessage email = MailMessage.Load(inputPath))
            {
                // Convert HTML body to plain text while preserving hyperlinks (URLs)
                string plainBody = email.GetHtmlBodyText(true);

                // Write the plain‑text result to a file
                try
                {
                    File.WriteAllText(outputPath, plainBody);
                    Console.WriteLine($"Plain‑text body saved to: {outputPath}");
                }
                catch (Exception writeEx)
                {
                    Console.Error.WriteLine($"Failed to write output file: {writeEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
