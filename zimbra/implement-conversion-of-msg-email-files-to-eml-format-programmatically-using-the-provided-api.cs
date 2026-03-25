using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input MSG file path
            string inputPath = "sample.msg";
            // Output EML file path
            string outputPath = "sample.eml";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Error: File not found – {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load MSG as MailMessage and save as EML
            using (Aspose.Email.MailMessage message = Aspose.Email.MailMessage.Load(inputPath))
            {
                message.Save(outputPath, Aspose.Email.SaveOptions.DefaultEml);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}