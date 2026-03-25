using System;
using System.IO;
using Aspose.Email.Storage.Zimbra;

namespace ZimbraTgzItemCount
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if (args.Length == 0)
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
                        using (Aspose.Email.Storage.Zimbra.TgzReader reader = new Aspose.Email.Storage.Zimbra.TgzReader(path))
                        {
                            int totalItems = reader.GetTotalItemsCount();
                            Console.WriteLine($"Archive: {path}");
                            Console.WriteLine($"Total items: {totalItems}");
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
}