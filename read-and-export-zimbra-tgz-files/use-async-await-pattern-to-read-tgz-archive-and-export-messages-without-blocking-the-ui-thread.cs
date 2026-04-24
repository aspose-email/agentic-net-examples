using Aspose.Email;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            // Validate arguments
            if (args.Length < 2)
            {
                Console.Error.WriteLine("Usage: <program> <tgzFilePath> <outputDirectory>");
                return;
            }

            string tgzFilePath = args[0];
            string outputDirectory = args[1];

            // Guard input file existence
            if (!File.Exists(tgzFilePath))
            {
                Console.Error.WriteLine($"Input TGZ file not found: {tgzFilePath}");
                return;
            }

            // Ensure output directory exists
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception dirEx)
            {
                Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                return;
            }

            // Read TGZ archive and export messages asynchronously
            try
            {
                using (TgzReader reader = new TgzReader(tgzFilePath))
                {
                    Console.WriteLine($"Total items in archive: {reader.GetTotalItemsCount()}");
                    await reader.ExportToAsync(outputDirectory, CancellationToken.None);
                    Console.WriteLine("Export completed successfully.");
                }
            }
            catch (Exception exportEx)
            {
                Console.Error.WriteLine($"Error during export: {exportEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
