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

            foreach (string filePath in args)
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
                        Console.WriteLine($"File: {filePath}");
                        Console.WriteLine($"Total items: {totalItems}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing file '{filePath}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}