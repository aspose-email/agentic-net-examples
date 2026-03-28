using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define input MSG file and output EMLX file paths
            string inputPath = "sample.msg";
            string outputPath = "sample.emlx";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the MSG file as a MapiMessage
            using (MapiMessage mapiMessage = MapiMessage.Load(inputPath))
            {
                // Convert the MapiMessage to a MailMessage preserving all properties
                MailConversionOptions conversionOptions = new MailConversionOptions();
                using (MailMessage mailMessage = mapiMessage.ToMailMessage(conversionOptions))
                {
                    // Prepare save options for EMLX format
                    EmlSaveOptions saveOptions = new EmlSaveOptions(MailMessageSaveType.EmlxFormat);

                    // Save the MailMessage as an EMLX file
                    mailMessage.Save(outputPath, saveOptions);
                    Console.WriteLine($"Message successfully saved as EMLX to: {outputPath}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
