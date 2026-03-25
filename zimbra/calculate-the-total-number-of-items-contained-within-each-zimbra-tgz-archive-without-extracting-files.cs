using System;
using System.IO;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the Zimbra TGZ archive (modify as needed)
            string tgzPath = "archive.tgz";

            // Verify that the file exists before attempting to read it
            if (!File.Exists(tgzPath))
            {
                Console.Error.WriteLine($"Error: File not found – {tgzPath}");
                return;
            }

            // Open the TGZ archive and retrieve the total items count
            try
            {
                using (TgzReader reader = new TgzReader(tgzPath))
                {
                    int totalItems = reader.GetTotalItemsCount();
                    Console.WriteLine($"Total items in '{tgzPath}': {totalItems}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing TGZ archive: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}