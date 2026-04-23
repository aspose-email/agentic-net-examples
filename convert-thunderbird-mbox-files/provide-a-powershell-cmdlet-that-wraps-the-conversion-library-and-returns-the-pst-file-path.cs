using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

namespace AsposeEmailMboxToPst
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                // Define input MBOX and output PST paths
                string mboxPath = "sample.mbox";
                string pstPath = "sample.pst";

                // Verify input MBOX file exists; create a minimal placeholder if missing
                if (!File.Exists(mboxPath))
                {
                    try
                    {
                        File.WriteAllText(mboxPath, string.Empty);
                        Console.WriteLine($"Created placeholder MBOX file at '{mboxPath}'.");
                    }
                    catch (Exception ioEx)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ioEx.Message}");
                        return;
                    }
                }

                // Ensure the directory for the PST file exists
                string pstDirectory = Path.GetDirectoryName(pstPath);
                if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
                {
                    try
                    {
                        Directory.CreateDirectory(pstDirectory);
                    }
                    catch (Exception dirEx)
                    {
                        Console.Error.WriteLine($"Failed to create PST directory: {dirEx.Message}");
                        return;
                    }
                }

                // Perform the conversion
                using (PersonalStorage pstStorage = MailStorageConverter.MboxToPst(mboxPath, pstPath))
                {
                    // Conversion succeeded; the PST file is now available at pstPath
                    Console.WriteLine($"Conversion completed. PST file path: {pstPath}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return;
            }
        }
    }
}
