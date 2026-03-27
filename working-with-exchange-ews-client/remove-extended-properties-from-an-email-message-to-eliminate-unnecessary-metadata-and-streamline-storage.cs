using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input and output file paths
            string inputPath = "input.msg";
            string outputPath = "output.msg";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the MSG file, remove all custom (extended) properties, and save
            using (MapiMessage message = MapiMessage.Load(inputPath))
            {
                // Retrieve custom properties collection
                var customProperties = message.GetCustomProperties();

                // Remove each custom property by its tag
                foreach (var kvp in customProperties)
                {
                    message.RemoveProperty(kvp.Key);
                }

                // Save the cleaned message
                message.Save(outputPath);
                Console.WriteLine($"Extended properties removed. Saved to: {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}