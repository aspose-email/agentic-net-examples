using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Input and output file paths
            string inputPath = "input.mhtml";
            string outputPath = "output.msg";

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

                Console.Error.WriteLine($"Input file '{inputPath}' not found.");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the MHTML message
            using (MailMessage message = MailMessage.Load(inputPath))
            {
                // Set email priority
                message.Priority = MailPriority.High;

                // Create custom SaveOptions for MSG format
                SaveOptions saveOptions = SaveOptions.CreateSaveOptions(MailMessageSaveType.OutlookMessageFormatUnicode);

                // Save as MSG with the custom options
                message.Save(outputPath, saveOptions);
            }

            Console.WriteLine("MHTML to MSG conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
