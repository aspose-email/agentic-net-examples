using System;
using System.IO;
using Aspose.Email.Storage.Zimbra;

namespace ZimbraTgzItemCounter
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args == null || args.Length == 0)
                {
                    Console.Error.WriteLine("No archive paths provided.");
                    return;
                }

                foreach (string archivePath in args)
                {
                    if (!File.Exists(archivePath))
                    {
                        Console.Error.WriteLine($"Error: File not found – {archivePath}");
                        continue;
                    }

                    try
                    {
                        using (TgzReader tgzReader = new TgzReader(archivePath))
                        {
                            int totalItems = tgzReader.GetTotalItemsCount();
                            Console.WriteLine($"Archive: {Path.GetFileName(archivePath)} – Total items: {totalItems}");
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
}
