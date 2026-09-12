using Aspose.Email.Storage.Pst;
using Aspose.Email;
using System;
using System.IO;
using System.Diagnostics;
using Aspose.Email.Storage;

namespace MboxToPstConversion
{
    // Author: Generated example for measuring MBOX to PST conversion time.
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define input MBOX file and output PST file paths.
                string mboxFilePath = "large_mailbox.mbox";
                string pstFilePath = "converted_mailbox.pst";

                // Guard: ensure the input MBOX file exists.
                if (!File.Exists(mboxFilePath))
                {
                    Console.Error.WriteLine($"Input MBOX file not found: {mboxFilePath}");
                    return;
                }

                // Guard: ensure the output directory exists.
                string outputDirectory = Path.GetDirectoryName(pstFilePath);
                if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Measure conversion time.
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                // Perform the conversion. The method returns a PersonalStorage instance.
                PersonalStorage pstStorage = MailStorageConverter.MboxToPst(mboxFilePath, pstFilePath);

                stopwatch.Stop();

                // Log the duration in seconds.
                Console.WriteLine($"Conversion completed in {stopwatch.Elapsed.TotalSeconds:F2} seconds.");

                // Optionally dispose the returned PersonalStorage if needed.
                if (pstStorage != null)
                {
                    pstStorage.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
