using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define input MSG file and output EML file paths
            string inputPath = "input.msg";
            string outputPath = "output.eml";

            // Verify that the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Error: File not found – {inputPath}");
                return;
            }

            try
            {
                // Load the MSG file into a MailMessage object
                using (MailMessage mail = MailMessage.Load(inputPath))
                {
                    // Save the message in EML format
                    mail.Save(outputPath, SaveOptions.DefaultEml);
                    Console.WriteLine($"Conversion successful: '{inputPath}' → '{outputPath}'.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}