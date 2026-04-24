using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the TGZ file and the directory where messages will be exported
            string tgzFilePath = "mailbox.tgz";
            string exportDirectory = "ExportedMail";

            // Verify that the TGZ file exists
            if (!File.Exists(tgzFilePath))
            {
                Console.Error.WriteLine($"Input TGZ file not found: {tgzFilePath}");
                return;
            }

            // Ensure the export directory exists
            if (!Directory.Exists(exportDirectory))
            {
                try
                {
                    Directory.CreateDirectory(exportDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create export directory: {dirEx.Message}");
                    return;
                }
            }

            // Open the TGZ reader and export its contents
            using (TgzReader tgzReader = new TgzReader(tgzFilePath))
            {
                try
                {
                    tgzReader.ExportTo(exportDirectory);
                    Console.WriteLine($"Export completed successfully to '{exportDirectory}'.");
                }
                catch (IOException ioEx)
                {
                    Console.Error.WriteLine($"I/O error during export: {ioEx.Message}");
                }
                catch (AsposeException aspEx)
                {
                    Console.Error.WriteLine($"Aspose.Email error: {aspEx.Message}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Unexpected error during export: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Fatal error: {ex.Message}");
        }
    }
}
