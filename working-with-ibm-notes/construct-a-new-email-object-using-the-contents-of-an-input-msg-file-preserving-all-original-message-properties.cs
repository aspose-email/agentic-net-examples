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
                Console.Error.WriteLine($"Error: File not found – {inputPath}");
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
                    Console.Error.WriteLine($"Error: Unable to create output directory – {ex.Message}");
                    return;
                }
            }

            // Load the MSG file into a MapiMessage and convert to MailMessage preserving all properties
            using (MapiMessage mapiMessage = MapiMessage.Load(inputPath))
            {
                MailConversionOptions conversionOptions = new MailConversionOptions();
                using (MailMessage mailMessage = mapiMessage.ToMailMessage(conversionOptions))
                {
                    // Save the new email object
                    mailMessage.Save(outputPath);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
