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
            string inputPath = "input.msg";
            string outputPath = "output.eml";

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
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Load MSG file, convert to MailMessage, and save as EML
            using (MapiMessage msg = MapiMessage.Load(inputPath))
            {
                MailConversionOptions conversionOptions = new MailConversionOptions();
                using (MailMessage mail = msg.ToMailMessage(conversionOptions))
                {
                    EmlSaveOptions emlOptions = new EmlSaveOptions(MailMessageSaveType.EmlFormat);
                    mail.Save(outputPath, emlOptions);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
