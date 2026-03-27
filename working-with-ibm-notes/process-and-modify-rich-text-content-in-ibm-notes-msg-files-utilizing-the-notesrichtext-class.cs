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
            string inputPath = "sample.msg";
            string outputPath = "sample_modified.msg";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Error: File not found – {inputPath}");
                return;
            }

            // Load the MSG file
            using (MapiMessage message = MapiMessage.Load(inputPath))
            {
                // Modify the rich‑text body (RTF)
                // Example: prepend a simple RTF header and text
                string rtfHeader = @"{\rtf1\ansi\deff0{\fonttbl{\f0\fswiss Helvetica;}}";
                string rtfContent = @"\fs24 This is modified rich text content.\par}";
                message.BodyRtf = rtfHeader + rtfContent;

                // Save the modified message
                try
                {
                    message.Save(outputPath);
                    Console.WriteLine($"Modified message saved to: {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error saving file – {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error – {ex.Message}");
        }
    }
}
