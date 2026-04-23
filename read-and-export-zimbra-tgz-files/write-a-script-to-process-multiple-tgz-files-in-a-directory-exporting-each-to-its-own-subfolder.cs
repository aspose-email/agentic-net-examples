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
            // Define the directory containing TGZ files.
            string inputDirectory = "InputTgz";

            // Ensure the input directory exists.
            if (!Directory.Exists(inputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(inputDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create input directory '{inputDirectory}': {ex.Message}");
                    return;
                }
            }

            // Get all TGZ files in the directory.
            string[] tgzFiles;
            try
            {
                tgzFiles = Directory.GetFiles(inputDirectory, "*.tgz");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to enumerate TGZ files: {ex.Message}");
                return;
            }

            // Process each TGZ file.
            foreach (string tgzPath in tgzFiles)
            {
                // Verify the TGZ file exists; if not, create a minimal placeholder.
                if (!File.Exists(tgzPath))
                {
                    try
                    {
                        // Create an empty TGZ file as a placeholder.
                        using (FileStream placeholder = File.Create(tgzPath))
                        {
                            // No content needed for an empty placeholder.
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder TGZ '{tgzPath}': {ex.Message}");
                        continue;
                    }
                }

                // Determine the output subfolder (named after the TGZ file without extension).
                string subfolderName = Path.GetFileNameWithoutExtension(tgzPath);
                string outputFolder = Path.Combine(inputDirectory, subfolderName);

                // Ensure the output subfolder exists.
                if (!Directory.Exists(outputFolder))
                {
                    try
                    {
                        Directory.CreateDirectory(outputFolder);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create output folder '{outputFolder}': {ex.Message}");
                        continue;
                    }
                }

                // Use TgzReader to export the contents of the TGZ file.
                try
                {
                    using (TgzReader reader = new TgzReader(tgzPath))
                    {
                        reader.ExportTo(outputFolder);
                        Console.WriteLine($"Exported '{tgzPath}' to '{outputFolder}'.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing TGZ '{tgzPath}': {ex.Message}");
                    // Continue with next file.
                }
            }
        }
        catch (Exception ex)
        {
            // Top-level exception guard.
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
