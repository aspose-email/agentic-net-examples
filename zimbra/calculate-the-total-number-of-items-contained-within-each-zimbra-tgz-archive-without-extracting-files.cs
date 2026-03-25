using System;
using System.IO;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string archivePath;
            if (args != null && args.Length > 0)
            {
                archivePath = args[0];
            }
            else
            {
                archivePath = "sample.tgz";
            }

            if (!File.Exists(archivePath))
            {
                Console.Error.WriteLine($"Error: File not found – {archivePath}");
                return;
            }

            using (Aspose.Email.Storage.Zimbra.TgzReader reader = new Aspose.Email.Storage.Zimbra.TgzReader(archivePath))
            {
                int totalItems = reader.GetTotalItemsCount();
                Console.WriteLine($"Total items in '{archivePath}': {totalItems}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}