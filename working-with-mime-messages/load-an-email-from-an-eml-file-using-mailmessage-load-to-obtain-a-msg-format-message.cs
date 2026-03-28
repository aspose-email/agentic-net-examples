using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            string inputFile = "sample.eml";
            string outputFile = "output.msg";

            // Ensure input file exists; create a minimal placeholder if missing
            if (!File.Exists(inputFile))
            {
                try
                {
                    string placeholder = "Subject: Placeholder\r\n\r\nThis is a placeholder email.";
                    File.WriteAllText(inputFile, placeholder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder EML file: {ex.Message}");
                    return;
                }
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputFile);
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

            // Load the EML file with options
            EmlLoadOptions loadOptions = new EmlLoadOptions
            {
                PreserveTnefAttachments = true,
                PreserveEmbeddedMessageFormat = true
            };

            using (MailMessage message = MailMessage.Load(inputFile, loadOptions))
            {
                // Save as MSG format
                try
                {
                    message.Save(outputFile, SaveOptions.DefaultMsg);
                    Console.WriteLine($"Message converted and saved to '{outputFile}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save MSG file: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
