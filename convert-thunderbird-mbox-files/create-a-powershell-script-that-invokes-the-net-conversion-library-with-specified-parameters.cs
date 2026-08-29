using System;
using System.IO;
using Aspose.Email;

// Author: Aspose.Email example author
class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "TestEml.eml";
            string outputPath = "output.msg";

            // Verify that the input EML file exists
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MailMessage placeholder = new MailMessage(
                        "sender@example.com",
                        "recipient@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath, SaveOptions.DefaultEml);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder message: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Initialize load options for the EML file
            EmlLoadOptions emlLoadOptions = new EmlLoadOptions()
            {
                PreserveTnefAttachments = true,
                PreserveEmbeddedMessageFormat = true
            };

            // Load the EML message with the specified options and convert it to MSG
            using (MailMessage message = MailMessage.Load(inputPath, emlLoadOptions))
            {
                message.Save(outputPath, SaveOptions.DefaultMsg);
            }

            Console.WriteLine("EML to MSG conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
