using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Input HTML file path
            const string inputPath = "input.html";
            // Output MHTML file path
            const string outputPath = "output.mht";

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
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the HTML document as a MailMessage
            using (MailMessage mail = MailMessage.Load(inputPath))
            {
                // Configure MHTML save options
                MhtSaveOptions saveOptions = new MhtSaveOptions
                {
                    SaveAllHeaders = false // retain all headers as per requirement
                };

                // Save as MHTML
                mail.Save(outputPath, saveOptions);
                Console.WriteLine($"MHTML file saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
