using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Amp;

class Program
{
    static void Main()
    {
        try
        {
            // Input and output file paths
            string inputPath = "input.eml";
            string outputPath = "output.eml";

            // Guard input file existence
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

            // Load the AMP message
            using (AmpMessage ampMessage = new AmpMessage())
            {
                using (FileStream fs = File.OpenRead(inputPath))
                {
                    ampMessage.Import(fs);
                }

                // Create an AmpImage component (or retrieve an existing one)
                AmpImage imageComponent = new AmpImage(200, 100);
                ampMessage.AddAmpComponent(imageComponent);

                // Update source URL and dimensions
                imageComponent.Src = "https://example.com/newimage.jpg";
                imageComponent.Attributes.Width = 400;
                imageComponent.Attributes.Height = 300;

                // Save the updated message
                ampMessage.Save(outputPath);
                Console.WriteLine($"AMP message saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
