using Aspose.Email.Storage.Pst;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;

// Author: Aspose.Email .NET example - MBOX to PST conversion with size limit check
namespace MboxToPstConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define source MBOX and target PST paths (adjust as needed)
                string mboxPath = "input.mbox";
                string pstPath = "output.pst";

                // Maximum allowed MBOX file size (e.g., 100 MB)
                const long maxSizeBytes = 100L * 1024L * 1024L;

                // Verify source MBOX file exists
                FileInfo mboxInfo;
                try
                {
                    mboxInfo = new FileInfo(mboxPath);
                    if (!mboxInfo.Exists)
                    {
                        Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                        return;
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error accessing MBOX file: {ex.Message}");
                    return;
                }

                // Abort if file size exceeds the defined limit
                if (mboxInfo.Length > maxSizeBytes)
                {
                    Console.Error.WriteLine($"MBOX file size ({mboxInfo.Length} bytes) exceeds the limit of {maxSizeBytes} bytes.");
                    return;
                }

                // Ensure the output directory exists
                string pstDirectory = Path.GetDirectoryName(pstPath);
                if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(pstDirectory);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create output directory '{pstDirectory}': {ex.Message}");
                        return;
                    }
                }

                // Perform the conversion
                try
                {
                    using (PersonalStorage pst = MailStorageConverter.MboxToPst(mboxPath, pstPath))
                    {
                        // The conversion is performed by the static method; the returned PST object is disposed here.
                    }

                    Console.WriteLine($"MBOX file successfully converted to PST: {pstPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Conversion failed: {ex.Message}");
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
