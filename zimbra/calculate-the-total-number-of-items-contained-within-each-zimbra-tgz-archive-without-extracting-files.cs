using System;
using System.IO;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            if (args == null || args.Length == 0)
            {
                Console.Error.WriteLine("Error: No archive paths provided.");
                return;
            }

            foreach (string archivePath in args)
            {
                if (string.IsNullOrWhiteSpace(archivePath))
                {
                    Console.Error.WriteLine("Error: Empty archive path.");
                    continue;
                }

                if (!File.Exists(archivePath))
                {
                    Console.Error.WriteLine($"Error: File not found – {archivePath}");
                    continue;
                }

                try
                {
                    using (TgzReader reader = new TgzReader(archivePath))
                    {
                        int totalItems = reader.GetTotalItemsCount();
                        Console.WriteLine($"Archive: {archivePath} - Total items: {totalItems}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing archive '{archivePath}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}