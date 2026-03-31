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
            string outputPath = "output.mht";

            // Ensure the input HTML file exists; create a minimal placeholder if missing.
            if (!File.Exists(inputPath))
            {
                try
                {
                    File.WriteAllText(inputPath, "<html><body><p>Placeholder</p></body></html>");
                    Console.WriteLine($"Created placeholder HTML file at '{inputPath}'.");
                }
                catch (Exception createEx)
                {
                    Console.Error.WriteLine($"Failed to create placeholder HTML file: {createEx.Message}");
                    return;
                }
            }

            // Ensure the output directory exists.
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

            // Load the HTML file into a MailMessage.
            using (MailMessage mailMessage = MailMessage.Load(inputPath, new HtmlLoadOptions()))
            {
                // Save the message as MHTML, preserving all resources.
                mailMessage.Save(outputPath, SaveOptions.DefaultMhtml);
            }

            Console.WriteLine($"HTML successfully converted to MHTML at '{outputPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
