using System;
using System.IO;
using Aspose.Email;

class Program
{
    // Author: Aspose.Email example – load EML and save as MHTML
    static void Main()
    {
        try
        {
            string inputPath = "source.eml";
            string outputPath = "target.mhtml";

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

            // Load the EML message
            using (MailMessage mailMessage = MailMessage.Load(inputPath))
            {
                // Save as MHTML using the default options
                mailMessage.Save(outputPath, SaveOptions.DefaultMhtml);
            }

            Console.WriteLine($"Message saved as MHTML to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
