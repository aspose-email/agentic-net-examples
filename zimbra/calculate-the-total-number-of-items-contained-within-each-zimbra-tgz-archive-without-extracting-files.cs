using System;
using System.IO;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string[] archivePaths;
            if (args != null && args.Length > 0)
            {
                archivePaths = args;
            }
            else
            {
                archivePaths = new string[] { "sample.tgz" };
            }

            foreach (string archivePath in archivePaths)
            {
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
                        Console.WriteLine($"Total items in '{archivePath}': {totalItems}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing '{archivePath}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}