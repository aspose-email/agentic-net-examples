using System;
using System.IO;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string folderPath = args.Length > 0 ? args[0] : "ZimbraArchives";

            if (!Directory.Exists(folderPath))
            {
                Console.Error.WriteLine($"Error: Directory not found – {folderPath}");
                return;
            }

            string[] tgzFiles = Directory.GetFiles(folderPath, "*.tgz");
            if (tgzFiles.Length == 0)
            {
                Console.WriteLine("No TGZ files found.");
                return;
            }

            foreach (string filePath in tgzFiles)
            {
                if (!File.Exists(filePath))
                {
                    Console.Error.WriteLine($"Error: File not found – {filePath}");
                    continue;
                }

                try
                {
                    using (TgzReader reader = new TgzReader(filePath))
                    {
                        int totalItems = reader.GetTotalItemsCount();
                        Console.WriteLine($"Archive: {Path.GetFileName(filePath)} – Total items: {totalItems}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing {filePath}: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}