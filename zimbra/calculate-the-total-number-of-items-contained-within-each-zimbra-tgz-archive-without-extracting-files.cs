using System;
using System.IO;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string[] archivePaths = args.Length > 0 ? args : new string[] { "sample1.tgz", "sample2.tgz" };

            foreach (string path in archivePaths)
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
                    Console.Error.WriteLine($"Error processing {path}: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}