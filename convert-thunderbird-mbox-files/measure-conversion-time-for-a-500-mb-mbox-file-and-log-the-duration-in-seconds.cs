using Aspose.Email;
using System;
using System.IO;
using System.Diagnostics;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Paths for the source MBOX file and the destination PST file
            string mboxFilePath = "C:\\Data\\500mb.mbox";
            string pstFilePath = "C:\\Data\\output.pst";

            // Verify that the source MBOX file exists
            if (!File.Exists(mboxFilePath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxFilePath}");
                return;
            }

            // Ensure the output directory exists
            string pstDirectory = Path.GetDirectoryName(pstFilePath);
            if (!Directory.Exists(pstDirectory))
            {
                Console.Error.WriteLine($"Output directory does not exist: {pstDirectory}");
                return;
            }

            // Measure conversion time
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Perform the conversion; the returned PersonalStorage must be disposed
            using (PersonalStorage pst = MailStorageConverter.MboxToPst(mboxFilePath, pstFilePath))
            {
                // No additional processing required
            }

            stopwatch.Stop();

            Console.WriteLine($"Conversion completed in {stopwatch.Elapsed.TotalSeconds} seconds.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
