using System;
using System.IO;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define the list of Zimbra TGZ archive files to process
            string[] archivePaths = new string[]
            {
                "archive1.tgz",
                "archive2.tgz"
                // Add more paths as needed
            };

            foreach (string archivePath in archivePaths)
            {
                // Verify that the file exists before attempting to open it
                if (!File.Exists(archivePath))
                {
                    Console.Error.WriteLine($"Error: File not found – {archivePath}");
                    continue;
                }

                try
                {
                    // Open the TGZ archive using TgzReader (IDisposable)
                    using (TgzReader reader = new TgzReader(archivePath))
                    {
                        // Retrieve the total number of items without extracting
                        int totalItems = reader.GetTotalItemsCount();

                        // Output the result
                        Console.WriteLine($"Archive: {Path.GetFileName(archivePath)} - Total items: {totalItems}");
                    }
                }
                catch (Exception ex)
                {
                    // Handle any errors that occur while processing the archive
                    Console.Error.WriteLine($"Error processing '{archivePath}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            // Top-level exception guard
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}