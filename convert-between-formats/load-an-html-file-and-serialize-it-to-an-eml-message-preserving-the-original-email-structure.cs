using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string inputPath = "input.html";
            string outputPath = "output.eml";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
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

            try
            {
                // Load HTML file as MailMessage with default options
                HtmlLoadOptions loadOptions = new HtmlLoadOptions();
                using (MailMessage mailMessage = MailMessage.Load(inputPath, loadOptions))
                {
                    // Save as EML preserving structure
                    EmlSaveOptions saveOptions = new EmlSaveOptions(MailMessageSaveType.EmlFormat);
                    mailMessage.Save(outputPath, saveOptions);
                    Console.WriteLine("HTML file successfully serialized to EML.");
                }
            }
            catch (Exception processingEx)
            {
                Console.Error.WriteLine($"Error processing email: {processingEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
