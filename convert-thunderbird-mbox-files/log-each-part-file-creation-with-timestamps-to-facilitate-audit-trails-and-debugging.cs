using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input PST file path
            string inputPstPath = "input.pst";

            // Ensure the input PST file exists; create a minimal placeholder if missing
            if (!File.Exists(inputPstPath))
            {
                try
                {
                    // Create an empty Unicode PST file as a placeholder
                    PersonalStorage.Create(inputPstPath, FileFormatVersion.Unicode);
                    Console.WriteLine($"{DateTime.Now:u} - Created placeholder PST file at '{inputPstPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Output directory for split parts
            string outputFolder = "PstParts";

            // Ensure the output directory exists
            try
            {
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                return;
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(inputPstPath))
            {
                // Define maximum size for each part (e.g., 10 MB)
                long maxPartSize = 10 * 1024 * 1024; // 10 MB

                // Split the PST into parts; parts will be created in the output folder
                try
                {
                    pst.SplitInto(maxPartSize, outputFolder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error during PST split: {ex.Message}");
                    return;
                }

                // Log each created part file with a timestamp
                try
                {
                    string[] partFiles = Directory.GetFiles(outputFolder, "*.pst");
                    foreach (string partFile in partFiles)
                    {
                        Console.WriteLine($"{DateTime.Now:u} - Created part file: {Path.GetFileName(partFile)}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to enumerate part files: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
