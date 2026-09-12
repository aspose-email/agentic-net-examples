using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Author note: This sample loads a TNEF file and exports it as a MSG file.
            string inputPath = "input.tnef";
            string outputPath = "output.msg";

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

            // Load the TNEF message
            MapiMessage tnefMessage = MapiMessage.LoadFromTnef(inputPath);

            // Convert to MailMessage to preserve attachments and save as MSG
            MailConversionOptions conversionOptions = new MailConversionOptions();
            MailMessage mailMessage = tnefMessage.ToMailMessage(conversionOptions);
            mailMessage.Save(outputPath, SaveOptions.DefaultMsg);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
