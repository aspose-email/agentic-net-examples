using Aspose.Email;
using System;
using System.IO;
using System.Diagnostics;
using Aspose.Email.Storage.Zimbra;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input TGZ archive path
            string tgzPath = "archive.tgz";

            // Output directory for exported messages
            string outputDirectory = "Exported";

            // Verify input file exists
            if (!File.Exists(tgzPath))
            {
                Console.Error.WriteLine($"Input file '{tgzPath}' does not exist.");
                return;
            }

            // Ensure output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Measure execution time
            Stopwatch stopwatch = Stopwatch.StartNew();

            // Read the TGZ archive and export its contents
            using (TgzReader reader = new TgzReader(tgzPath))
            {
                reader.ExportTo(outputDirectory);
            }

            stopwatch.Stop();

            Console.WriteLine($"Total execution time: {stopwatch.Elapsed}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
