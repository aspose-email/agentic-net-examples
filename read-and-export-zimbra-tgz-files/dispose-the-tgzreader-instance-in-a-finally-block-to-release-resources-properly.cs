using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Zimbra;

namespace TgzReaderExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define input TGZ file and output directory
                string tgzFilePath = "mailbox.tgz";
                string outputDirectory = "ExportedMessages";

                // Verify that the TGZ file exists
                if (!File.Exists(tgzFilePath))
                {
                    Console.Error.WriteLine($"Input file not found: {tgzFilePath}");
                    return;
                }

                // Ensure the output directory exists
                if (!Directory.Exists(outputDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }
                    catch (Exception dirEx)
                    {
                        Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                        return;
                    }
                }

                // Initialize TgzReader and ensure it is disposed in finally block
                TgzReader tgzReader = null;
                try
                {
                    tgzReader = new TgzReader(tgzFilePath);
                    // Export all messages and folder structure to the output directory
                    tgzReader.ExportTo(outputDirectory);
                    Console.WriteLine($"Export completed to: {outputDirectory}");
                }
                finally
                {
                    // Dispose the reader to release unmanaged resources
                    if (tgzReader != null)
                    {
                        tgzReader.Dispose();
                    }
                }
            }
            catch (Exception ex)
            {
                // Global exception handling
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
