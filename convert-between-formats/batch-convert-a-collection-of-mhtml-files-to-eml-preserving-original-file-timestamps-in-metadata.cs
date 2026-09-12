using System;
using System.IO;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            // Input directory containing MHTML files
            string inputDir = "MhtmlFiles";
            // Output directory for generated EML files
            string outputDir = "EmlOutput";

            // Verify input directory exists
            if (!Directory.Exists(inputDir))
            {
                Console.Error.WriteLine($"Input directory '{inputDir}' does not exist.");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory '{outputDir}': {ex.Message}");
                    return;
                }
            }

            // Get all .mht and .mhtml files
            string[] mhtmlFiles = Directory.GetFiles(inputDir, "*.mht");
            string[] mhtmlFilesAlt = Directory.GetFiles(inputDir, "*.mhtml");
            string[] allFiles = new string[mhtmlFiles.Length + mhtmlFilesAlt.Length];
            mhtmlFiles.CopyTo(allFiles, 0);
            mhtmlFilesAlt.CopyTo(allFiles, mhtmlFiles.Length);

            foreach (string mhtmlPath in allFiles)
            {
                try
                {
                    // Preserve original file timestamps
                    DateTime originalWriteTime = File.GetLastWriteTime(mhtmlPath);

                    // Load MHTML file into MailMessage
                    using (MailMessage message = MailMessage.Load(mhtmlPath))
                    {
                        // Set the message Date header to the original file timestamp
                        message.Date = originalWriteTime;

                        // Determine output file name with .eml extension
                        string fileNameWithoutExt = Path.GetFileNameWithoutExtension(mhtmlPath);
                        string emlPath = Path.Combine(outputDir, fileNameWithoutExt + ".eml");

                        // Save as EML; format inferred from extension
                        message.Save(emlPath);
                    }
                }
                catch (Exception exFile)
                {
                    Console.Error.WriteLine($"Error processing '{mhtmlPath}': {exFile.Message}");
                    // Continue with next file
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
