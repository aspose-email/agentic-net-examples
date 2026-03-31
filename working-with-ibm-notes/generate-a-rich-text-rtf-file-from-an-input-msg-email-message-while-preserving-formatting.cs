using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "input.msg";
            string outputPath = "output.rtf";

            // Ensure input MSG exists; create a minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "Placeholder Subject",
                        "Placeholder body text.",
                        "sender@example.com",
                        "receiver@example.com"))
                    {
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }
            }

            // Load the MSG and extract RTF body
            try
            {
                using (MapiMessage message = MapiMessage.Load(inputPath))
                {
                    string rtfContent = message.BodyRtf ?? string.Empty;

                    // Ensure output directory exists
                    string outputDir = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }

                    // Write RTF content to file
                    File.WriteAllText(outputPath, rtfContent);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing MSG file: {ex.Message}");
                return;
            }

            Console.WriteLine($"RTF file saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
