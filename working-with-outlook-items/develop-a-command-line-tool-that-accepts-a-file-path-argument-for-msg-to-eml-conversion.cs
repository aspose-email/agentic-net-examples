using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            if (args == null || args.Length == 0)
            {
                Console.Error.WriteLine("Please provide the path to the MSG file as an argument.");
                return;
            }

            string inputPath = args[0];

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

                Console.Error.WriteLine($"Input file does not exist: {inputPath}");
                return;
            }

            string outputDirectory = Path.GetDirectoryName(inputPath);
            if (string.IsNullOrEmpty(outputDirectory))
            {
                outputDirectory = Directory.GetCurrentDirectory();
            }

            if (!Directory.Exists(outputDirectory))
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

            string outputPath = Path.ChangeExtension(inputPath, ".eml");

            // Load MSG as MailMessage and save as EML
            using (MailMessage message = MailMessage.Load(inputPath))
            {
                message.Save(outputPath, SaveOptions.DefaultEml);
            }

            Console.WriteLine($"Conversion completed successfully. EML saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
