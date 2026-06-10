using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static void Main()
    {
        try
        {
            // Define the paths to the Zimbra TGZ archives.
            string[] tgzPaths = new string[]
            {
                "archive1.tgz",
                "archive2.tgz"
                // Add more archive paths as needed.
            };

            foreach (string tgzPath in tgzPaths)
            {
                // Guard against missing files.
                if (!File.Exists(tgzPath))
                {
                    Console.Error.WriteLine($"File not found: {tgzPath}");
                    continue;
                }

                // Open the TGZ archive and retrieve the total items count.
                using (TgzReader reader = new TgzReader(tgzPath))
                {
                    int totalItems = reader.GetTotalItemsCount();
                    Console.WriteLine($"Archive: {tgzPath}");
                    Console.WriteLine($"Total items: {totalItems}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
