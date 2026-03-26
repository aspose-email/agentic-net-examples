using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input MSG file path
            string inputPath = "sample.msg";
            // Output HTML file path
            string outputPath = "output.html";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the MSG file into a MapiMessage
            using (MapiMessage mapiMessage = MapiMessage.Load(inputPath))
            {
                // Convert MapiMessage to MailMessage with default conversion options
                MailMessage mailMessage = mapiMessage.ToMailMessage(new MailConversionOptions());

                // Ensure MailMessage is disposed after use
                using (mailMessage)
                {
                    // Save the MailMessage as HTML (MHTML format)
                    mailMessage.Save(outputPath, SaveOptions.DefaultMhtml);
                }
            }
        }
        catch (Exception ex)
        {
            // Log any unexpected errors
            Console.Error.WriteLine(ex.Message);
        }
    }
}