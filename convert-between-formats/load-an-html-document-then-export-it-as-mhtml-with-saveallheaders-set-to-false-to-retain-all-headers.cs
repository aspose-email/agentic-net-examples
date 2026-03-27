using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.html";
            string outputPath = "output.mht";

            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Error: Input file not found – {inputPath}");
                return;
            }

            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Console.Error.WriteLine($"Error: Output directory does not exist – {outputDirectory}");
                return;
            }

            using (MailMessage message = MailMessage.Load(inputPath))
            {
                MhtSaveOptions saveOptions = new MhtSaveOptions();
                saveOptions.SaveAllHeaders = false;

                message.Save(outputPath, saveOptions);
                Console.WriteLine($"MHTML file saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
