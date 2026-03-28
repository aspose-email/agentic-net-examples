using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Zimbra;

namespace ZimbraTgzItemCount
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

                foreach (string arg in args)
                {
                    string filePath = arg;
                    if (!File.Exists(filePath))
                    {
                        Console.Error.WriteLine($"Error: File not found – {filePath}");
                        continue;
                    }

                    try
                    {
                        using (TgzReader reader = new TgzReader(filePath))
                        {
                            int totalItemsCount = reader.GetTotalItemsCount();
                            Console.WriteLine($"Archive: {Path.GetFileName(filePath)} – Total items: {totalItemsCount}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error processing '{filePath}': {ex.Message}");
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
