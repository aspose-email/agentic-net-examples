using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Input MSG file path
            string inputPath = @"c:\outlookmessage.msg";
            // Output MSG file path (attachment‑free copy)
            string outputPath = @"c:\outlookmessage_stripped.msg";

            // Verify the input file exists
            if (!File.Exists(inputPath))
            {
                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Create a copy of the original message to preserve the original file
            try
            {
                File.Copy(inputPath, outputPath, true);
            }
            catch (Exception copyEx)
            {
                Console.Error.WriteLine($"Failed to copy file: {copyEx.Message}");
                return;
            }

            // Remove all attachments from the copied MSG file
            // This static method modifies the file in place
            MapiMessage.DestroyAttachments(outputPath);

            Console.WriteLine("All attachments have been stripped from the message.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
