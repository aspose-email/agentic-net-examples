using Aspose.Email;
using System;
using System.IO;
using System.IO.Compression;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for input TGZ, extraction folder, and output TGZ
            string inputTgzPath = "input.tgz";
            string extractionFolder = "extracted_emails";
            string outputTgzPath = "exported.tgz";

            // Verify the input TGZ file exists
            if (!File.Exists(inputTgzPath))
            {
                Console.Error.WriteLine($"Input TGZ file not found: {inputTgzPath}");
                return;
            }

            // Ensure the extraction folder exists (create if necessary)
            try
            {
                if (!Directory.Exists(extractionFolder))
                {
                    Directory.CreateDirectory(extractionFolder);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create extraction folder: {dirEx.Message}");
                return;
            }

            // Extract the TGZ archive using Aspose.Email's TgzReader
            try
            {
                using (TgzReader tgzReader = new TgzReader(inputTgzPath))
                {
                    tgzReader.ExportTo(extractionFolder);
                }
                Console.WriteLine($"Extraction completed to folder: {extractionFolder}");
            }
            catch (Exception extractEx)
            {
                Console.Error.WriteLine($"Error during extraction: {extractEx.Message}");
                return;
            }

            // Compress the extracted emails into a new TGZ archive
            try
            {
                // Ensure there is something to compress
                if (!Directory.Exists(extractionFolder) ||
                    Directory.GetFiles(extractionFolder, "*", SearchOption.AllDirectories).Length == 0)
                {
                    Console.Error.WriteLine("No files found to compress.");
                    return;
                }

                // Create the output TGZ file (gzip compression of concatenated files)
                using (FileStream outFileStream = new FileStream(outputTgzPath, FileMode.Create, FileAccess.Write))
                using (GZipStream gzipStream = new GZipStream(outFileStream, CompressionMode.Compress))
                {
                    // Iterate through all files in the extraction folder
                    string[] allFiles = Directory.GetFiles(extractionFolder, "*", SearchOption.AllDirectories);
                    foreach (string filePath in allFiles)
                    {
                        // Write a simple separator (file name) for readability (optional)
                        string header = $"---{Path.GetFileName(filePath)}---{Environment.NewLine}";
                        byte[] headerBytes = System.Text.Encoding.UTF8.GetBytes(header);
                        gzipStream.Write(headerBytes, 0, headerBytes.Length);

                        // Read file bytes and write to the gzip stream
                        byte[] fileBytes = File.ReadAllBytes(filePath);
                        gzipStream.Write(fileBytes, 0, fileBytes.Length);
                    }
                }

                Console.WriteLine($"Compressed TGZ created at: {outputTgzPath}");
            }
            catch (Exception compressEx)
            {
                Console.Error.WriteLine($"Error during compression: {compressEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
