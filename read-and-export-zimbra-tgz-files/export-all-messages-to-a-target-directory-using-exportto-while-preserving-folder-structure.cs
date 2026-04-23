using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "archive.tgz";
            string outputPath = "ExportedMessages";

            // Ensure input file exists; create minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                try
                {
                    File.WriteAllBytes(inputPath, new byte[0]);
                    Console.WriteLine($"Placeholder file created at '{inputPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder file: {ex.Message}");
                    return;
                }
            }

            // Ensure output directory exists
            try
            {
                if (!Directory.Exists(outputPath))
                {
                    Directory.CreateDirectory(outputPath);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            // Open the TGZ archive and export its contents preserving folder structure
            try
            {
                using (FileStream fileStream = File.OpenRead(inputPath))
                {
                    using (TgzReader reader = new TgzReader(fileStream))
                    {
                        reader.ExportTo(outputPath);
                        Console.WriteLine($"Messages exported to '{outputPath}'.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error during export: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
