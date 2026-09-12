using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace TnefToMsgConverter
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Input TNEF file path
                string inputPath = "sample.tnef";
                // Desired output MSG file path
                string outputPath = "sample.msg";

                // Verify input file exists
                if (!File.Exists(inputPath))
                {
                    Console.Error.WriteLine($"Input file not found: {inputPath}");
                    return;
                }

                // Ensure output directory exists
                string outputDirectory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Load the TNEF file into a MapiMessage
                MapiMessage mapMessage = MapiMessage.LoadFromTnef(inputPath);

                // Save the message as MSG, preserving all attachments and metadata
                mapMessage.Save(outputPath, SaveOptions.DefaultMsg);
            }
            catch (Exception ex)
            {
                // Log any unexpected errors
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
