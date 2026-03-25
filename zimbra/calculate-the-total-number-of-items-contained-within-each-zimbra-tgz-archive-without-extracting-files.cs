using System;
using System.IO;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Path to the Zimbra TGZ archive (argument or default)
            string tgzPath = args.Length > 0 ? args[0] : "archive.tgz";

            // Verify the file exists before attempting to read it
            if (!File.Exists(tgzPath))
            {
                Console.Error.WriteLine($"Error: File not found – {tgzPath}");
                return;
            }

            // Open the TGZ archive and retrieve the total item count
            using (TgzReader reader = new TgzReader(tgzPath))
            {
                int totalItems = reader.GetTotalItemsCount();
                Console.WriteLine($"Total items in '{tgzPath}': {totalItems}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}