using System;
using System.IO;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = @"input.msg";
            string outputPath = @"output.msg";

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
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }
            }

            // Remove all attachments from the MSG file
            try
            {
                MapiAttachmentCollection removedAttachments = MapiMessage.RemoveAttachments(inputPath);
                // Optionally, you can inspect removedAttachments here
            }
            catch (Exception removeEx)
            {
                Console.Error.WriteLine($"Failed to remove attachments: {removeEx.Message}");
                return;
            }

            // Load the modified message and save to a new file
            try
            {
                using (MapiMessage message = MapiMessage.Load(inputPath))
                {
                    message.Save(outputPath);
                }
                Console.WriteLine($"Attachments stripped successfully. Saved to: {outputPath}");
            }
            catch (Exception loadSaveEx)
            {
                Console.Error.WriteLine($"Error loading or saving message: {loadSaveEx.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
