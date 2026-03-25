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

            foreach (string path in args)
            {
                if (!File.Exists(path))
                {
                    Console.Error.WriteLine($"Error: File not found – {path}");
                    continue;
                }

                try
                {
                    using (TgzReader reader = new TgzReader(path))
                    {
                        int totalItems = reader.GetTotalItemsCount();
                        Console.WriteLine($"Archive: {Path.GetFileName(path)} - Total items: {totalItems}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing '{path}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}