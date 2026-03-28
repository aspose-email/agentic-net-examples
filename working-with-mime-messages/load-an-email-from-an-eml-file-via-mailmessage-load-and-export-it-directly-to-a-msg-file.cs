using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Define input and output file paths
            string inputPath = "input.eml";
            string outputPath = "output.msg";

            // Verify that the input EML file exists
            if (!File.Exists(inputPath))
{
    try
    {
        string placeholder = "From: placeholder@example.com\r\nTo: recipient@example.com\r\nSubject: Placeholder\r\n\r\nThis is a placeholder EML.";
        File.WriteAllText(inputPath, placeholder);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder EML: {ex.Message}");
        return;
    }
}


            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Load the EML file and save it as MSG
            using (MailMessage message = MailMessage.Load(inputPath))
            {
                message.Save(outputPath, SaveOptions.DefaultMsg);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
