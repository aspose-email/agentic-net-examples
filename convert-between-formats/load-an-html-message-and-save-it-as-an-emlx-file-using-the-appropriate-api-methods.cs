using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string inputPath = "input.html";
            string outputPath = "output.emlx";

            // Ensure the input HTML file exists; create a minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                try
                {
                    File.WriteAllText(inputPath, "<html><body><p>Placeholder</p></body></html>");
                    Console.WriteLine($"Created placeholder input file: {inputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder input file: {ex.Message}");
                    return;
                }
            }

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Load the HTML message
            HtmlLoadOptions htmlLoadOptions = new HtmlLoadOptions();
            using (MailMessage mailMessage = MailMessage.Load(inputPath, htmlLoadOptions))
            {
                // Prepare save options for EMLX format
                EmlSaveOptions emlSaveOptions = new EmlSaveOptions(MailMessageSaveType.EmlxFormat);
                // Save the message as EMLX
                mailMessage.Save(outputPath, emlSaveOptions);
            }

            Console.WriteLine($"Message saved as EMLX to {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
