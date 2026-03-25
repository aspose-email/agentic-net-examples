using System;
using System.IO;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string tgzPath = args.Length > 0 ? args[0] : "archive.tgz";

            if (!File.Exists(tgzPath))
            {
                Console.Error.WriteLine($"Error: File not found – {tgzPath}");
                return;
            }

            using (Aspose.Email.Storage.Zimbra.TgzReader reader = new Aspose.Email.Storage.Zimbra.TgzReader(tgzPath))
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