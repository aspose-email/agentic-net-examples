using System;
using System.IO;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        // Top-level exception guard
        try
        {
            // Input MSG file path
            string inputPath = "input.msg";
            // Output RTF file path
            string outputPath = "output.rtf";

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

            // Load the MSG file and extract RTF body
            using (MapiMessage message = MapiMessage.Load(inputPath))
            {
                string rtfBody = message.BodyRtf;
                if (string.IsNullOrEmpty(rtfBody))
                {
                    Console.Error.WriteLine("Error: No RTF body found in the message.");
                    return;
                }

                // Save the RTF content to a file
                File.WriteAllText(outputPath, rtfBody);
                Console.WriteLine($"RTF content saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
