using System;
using System.IO;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            if (args.Length == 0)
            {
                Console.Error.WriteLine("Please provide at least one TGZ file path as an argument.");
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
                    using (Aspose.Email.Storage.Zimbra.TgzReader reader = new Aspose.Email.Storage.Zimbra.TgzReader(path))
                    {
                        int totalItems = reader.GetTotalItemsCount();
                        Console.WriteLine($"File: {Path.GetFileName(path)} - Total items: {totalItems}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing file {path}: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}