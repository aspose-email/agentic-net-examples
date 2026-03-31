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
            string outputPath = "output.msg";

            // Ensure input MSG file exists; create a minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                try
                {
                    MapiMessage placeholder = new MapiMessage("sender@example.com", "recipient@example.com", "Placeholder Subject", "Placeholder body");
                    placeholder.Save(inputPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the MSG file and modify its rich text (RTF) content
            MapiMessage message;
            try
            {
                message = MapiMessage.Load(inputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error loading MSG file: {ex.Message}");
                return;
            }

            // Modify the RTF body if present; otherwise set a new RTF body
            string originalRtf = message.BodyRtf ?? string.Empty;
            string appendedRtf = originalRtf + @"\par Modified by Aspose.Email";
            try
            {
                message.SetBodyRtf(appendedRtf, true);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error modifying RTF body: {ex.Message}");
                return;
            }

            // Save the modified message
            try
            {
                message.Save(outputPath);
                Console.WriteLine($"Modified MSG saved to: {outputPath}");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error saving modified MSG file: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
