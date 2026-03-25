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
                    Console.Error.WriteLine("Error: No archive paths provided.");
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
                        using (Aspose.Email.Storage.Zimbra.TgzReader reader = new Aspose.Email.Storage.Zimbra.TgzReader(archivePath))
                        {
                            int totalItems = reader.GetTotalItemsCount();
                            Console.WriteLine($"Archive: {archivePath}");
                            Console.WriteLine($"Total items: {totalItems}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error processing \"{archivePath}\": {ex.Message}");
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