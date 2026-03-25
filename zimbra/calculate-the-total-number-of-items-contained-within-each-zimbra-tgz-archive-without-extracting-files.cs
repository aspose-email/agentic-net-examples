using System;
using System.IO;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string tgzPath;
            if (args != null && args.Length > 0 && !string.IsNullOrWhiteSpace(args[0]))
            {
                tgzPath = args[0];
            }
            else
            {
                tgzPath = "archive.tgz";
            }

            if (!File.Exists(tgzPath))
            {
                Console.Error.WriteLine($"Error: File not found – {tgzPath}");
                return;
            }

            using (TgzReader reader = new TgzReader(tgzPath))
            {
                int totalItems = reader.GetTotalItemsCount();
                Console.WriteLine($"Total items in '{tgzPath}': {totalItems}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}