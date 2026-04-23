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
            string inputPath = "template.oft";
            string outputPath = "output.rtf";

            // Ensure the input OFT file exists; create a minimal placeholder if missing.
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage())
                    {
                        placeholder.BodyRtf = @"{\rtf1\ansi This is a placeholder OFT template.}";
                        placeholder.SaveAsTemplate(inputPath);
                        Console.WriteLine($"Placeholder OFT created at '{inputPath}'.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder OFT: {ex.Message}");
                    return;
                }
            }

            // Load the OFT template.
            MapiMessage oftMessage;
            try
            {
                oftMessage = MapiMessage.Load(inputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load OFT template: {ex.Message}");
                return;
            }

            // Extract the RTF body.
            string rtfContent = oftMessage.BodyRtf;
            if (string.IsNullOrEmpty(rtfContent))
            {
                Console.Error.WriteLine("The OFT template does not contain RTF body content.");
                return;
            }

            // Write the RTF content to the output file.
            try
            {
                File.WriteAllText(outputPath, rtfContent);
                Console.WriteLine($"RTF document saved to '{outputPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to write RTF file: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
