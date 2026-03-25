using System;
using System.IO;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static void Main()
    {
        try
        {
            // List of Zimbra TGZ archive file paths
            string[] archivePaths = new string[] { "archive1.tgz", "archive2.tgz" };

            foreach (string path in archivePaths)
            {
                // Verify that the archive file exists
                if (!File.Exists(path))
                {
                    Console.Error.WriteLine($"Error: File not found – {path}");
                    continue;
                }

                try
                {
                    // Open the TGZ archive without extracting its contents
                    using (TgzReader reader = new TgzReader(path))
                    {
                        // Retrieve the total number of items in the archive
                        int totalItems = reader.GetTotalItemsCount();
                        Console.WriteLine($"Archive: {Path.GetFileName(path)} – Total items: {totalItems}");
                    }
                }
                catch (Exception ex)
                {
                    // Handle any errors that occur while processing the archive
                    Console.Error.WriteLine($"Error processing {path}: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            // Catch any unexpected errors
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}