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
            // Paths for the input TGZ file and the output directory
            string tgzFilePath = "sample.tgz";
            string outputDirectory = "output";

            // Ensure the input TGZ file exists; create a minimal placeholder if missing
            if (!File.Exists(tgzFilePath))
            {
                // Create an empty placeholder TGZ file
                using (FileStream placeholderStream = File.Create(tgzFilePath))
                {
                    // No content needed for the placeholder
                }
                Console.WriteLine($"Placeholder TGZ file created at: {tgzFilePath}");
            }

            // Ensure the output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
                Console.WriteLine($"Output directory created at: {outputDirectory}");
            }

            // Define a custom buffer size (e.g., 256 KB) to improve performance on large TGZ files
            const int customBufferSize = 256 * 1024;

            // Open the TGZ file with a buffered stream using the custom buffer size
            using (FileStream fileStream = new FileStream(tgzFilePath, FileMode.Open, FileAccess.Read))
            using (BufferedStream bufferedStream = new BufferedStream(fileStream, customBufferSize))
            using (TgzReader tgzReader = new TgzReader(bufferedStream))
            {
                // Export all messages and directory structure to the specified output folder
                tgzReader.ExportTo(outputDirectory);
                Console.WriteLine($"TGZ contents exported to: {outputDirectory}");
            }
        }
        catch (Exception ex)
        {
            // Write any errors to the error console and exit gracefully
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
