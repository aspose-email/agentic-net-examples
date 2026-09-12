using Aspose.Email.Storage.Pst;
using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage;

// Author: Example demonstrating MBOX to PST conversion with size validation
class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Expect: args[0] = input MBOX path, args[1] = output PST path, args[2] = max size in bytes
            if (args.Length < 3)
            {
                Console.Error.WriteLine("Usage: <program> <mboxPath> <pstPath> <maxSizeBytes>");
                return;
            }

            string mboxPath = args[0];
            string pstPath = args[1];
            string maxSizeString = args[2];

            if (!long.TryParse(maxSizeString, out long maxSizeBytes))
            {
                Console.Error.WriteLine("Invalid maximum size value.");
                return;
            }

            // Guard: ensure input MBOX file exists
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Input MBOX file not found: {mboxPath}");
                return;
            }

            // Guard: ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }
            }

            // Convert MBOX to PST
            using (PersonalStorage pst = MailStorageConverter.MboxToPst(mboxPath, pstPath))
            {
                // Conversion completed; PersonalStorage will be disposed automatically
            }

            // Validate PST file size against the user‑specified maximum
            FileInfo pstInfo = new FileInfo(pstPath);
            long pstSizeBytes = pstInfo.Length;

            if (pstSizeBytes > maxSizeBytes)
            {
                Console.WriteLine($"PST size {pstSizeBytes} bytes exceeds the allowed maximum of {maxSizeBytes} bytes.");
            }
            else
            {
                Console.WriteLine($"PST size {pstSizeBytes} bytes is within the allowed limit of {maxSizeBytes} bytes.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
