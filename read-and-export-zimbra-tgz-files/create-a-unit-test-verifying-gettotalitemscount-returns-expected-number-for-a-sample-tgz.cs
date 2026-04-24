using Aspose.Email;
using System;
using System.IO;
using System.IO.Compression;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static void Main()
    {
        try
        {
            const string tgzPath = "sample.tgz";
            const int expectedCount = 0;

            // Ensure the TGZ file exists; create an empty placeholder if it does not.
            if (!File.Exists(tgzPath))
            {
                try
                {
                    using (FileStream fileStream = new FileStream(tgzPath, FileMode.Create, FileAccess.Write))
                    using (GZipStream gzipStream = new GZipStream(fileStream, CompressionMode.Compress))
                    {
                        // Write no data – results in an empty TGZ archive.
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder TGZ file: {ex.Message}");
                    return;
                }
            }

            // Read the TGZ archive and verify the total items count.
            try
            {
                using (TgzReader reader = new TgzReader(tgzPath))
                {
                    int totalItems = reader.GetTotalItemsCount();
                    Console.WriteLine($"Total items reported by TgzReader: {totalItems}");

                    if (totalItems != expectedCount)
                    {
                        Console.Error.WriteLine($"Test failed: expected {expectedCount} items, but got {totalItems}.");
                    }
                    else
                    {
                        Console.WriteLine("Test passed.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error while reading TGZ file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
