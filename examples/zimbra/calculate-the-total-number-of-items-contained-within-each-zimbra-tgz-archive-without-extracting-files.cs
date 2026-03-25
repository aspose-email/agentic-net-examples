using System;
using System.IO;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the Zimbra TGZ archive
            string archivePath = "sample.tgz";

            // Verify that the file exists before attempting to read it
            if (!File.Exists(archivePath))
            {
                Console.Error.WriteLine($"Error: File not found – {archivePath}");
                return;
            }

            // Open the archive and retrieve the total items count
            using (TgzReader reader = new TgzReader(archivePath))
            {
                int totalItems = reader.GetTotalItemsCount();
                Console.WriteLine($"Total items in '{archivePath}': {totalItems}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}