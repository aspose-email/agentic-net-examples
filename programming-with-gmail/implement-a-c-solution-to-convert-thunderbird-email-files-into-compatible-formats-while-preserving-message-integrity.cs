using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Storage.Mbox;

namespace ThunderbirdConversionSample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define input Thunderbird mbox file and output PST file paths
                string mboxFilePath = "thunderbird.mbox";
                string pstFilePath = "converted.pst";

                // Verify input mbox file exists
                if (!File.Exists(mboxFilePath))
                {
                    Console.Error.WriteLine($"Error: Input mbox file not found – {mboxFilePath}");
                    return;
                }

                // Ensure output directory exists
                try
                {
                    string outputDirectory = Path.GetDirectoryName(pstFilePath);
                    if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
                    {
                        Directory.CreateDirectory(outputDirectory);
                    }
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Error creating output directory: {dirEx.Message}");
                    return;
                }

                // Perform conversion from mbox to PST
                try
                {
                    // The conversion method returns a PersonalStorage instance representing the PST
                    using (PersonalStorage pstStorage = MailStorageConverter.MboxToPst(mboxFilePath, pstFilePath))
                    {
                        // Optionally, you can work with the PST storage here (e.g., list folders)
                        // For this sample we simply confirm the conversion succeeded.
                        Console.WriteLine($"Conversion succeeded. PST saved to: {pstFilePath}");
                    }
                }
                catch (Exception convEx)
                {
                    Console.Error.WriteLine($"Conversion failed: {convEx.Message}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                return;
            }
        }
    }
}
