using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

// Author: Aspose.Email example - load MSG, create new MailMessage preserving all properties
class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.msg";
            string outputPath = "output.msg";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the original MSG file
            using (MapiMessage originalMsg = MapiMessage.Load(inputPath))
            {
                // Convert to MailMessage preserving all properties
                MailConversionOptions conversionOptions = new MailConversionOptions();
                using (MailMessage newMsg = originalMsg.ToMailMessage(conversionOptions))
                {
                    // Save the new message (properties are retained)
                    newMsg.Save(outputPath);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
