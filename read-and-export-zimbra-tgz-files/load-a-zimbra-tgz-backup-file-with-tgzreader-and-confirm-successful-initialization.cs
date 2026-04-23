using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string tgzPath = "backup.tgz";

            if (!File.Exists(tgzPath))
            {
                Console.Error.WriteLine($"File not found: {tgzPath}");
                return;
            }

            using (TgzReader reader = new TgzReader(tgzPath))
            {
                int totalItems = reader.GetTotalItemsCount();
                Console.WriteLine($"Total items in TGZ backup: {totalItems}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
