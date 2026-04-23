using Aspose.Email.Storage.Pst;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string mboxPath = "source.mbox";
            string pstPath = "output.pst";

            // Verify source MBOX file exists
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            // Ensure the directory for the PST file exists
            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                Directory.CreateDirectory(pstDirectory);
            }

            try
            {
                // Convert MBOX to PST with proper disposal of the resulting PersonalStorage
                using (PersonalStorage pst = MailStorageConverter.MboxToPst(mboxPath, pstPath))
                {
                    Console.WriteLine("MBOX to PST conversion completed successfully.");
                }
            }
            catch (IOException ioEx)
            {
                // Handles file access issues such as locked source file
                Console.Error.WriteLine($"File access error: {ioEx.Message}");
                return;
            }
            catch (AsposeException aspEx)
            {
                // Handles Aspose.Email specific errors
                Console.Error.WriteLine($"Aspose.Email error: {aspEx.Message}");
                return;
            }
            catch (Exception ex)
            {
                // Handles any other unexpected errors
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            // Top-level guard for any unhandled exceptions
            Console.Error.WriteLine($"Fatal error: {ex.Message}");
        }
    }
}
