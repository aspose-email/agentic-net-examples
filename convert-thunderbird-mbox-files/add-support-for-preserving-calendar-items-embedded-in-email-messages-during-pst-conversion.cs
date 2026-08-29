using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

// Author: Aspose.Email .NET sample for MBOX to PST conversion
class Program
{
    static void Main()
    {
        try
        {
            // Paths – adjust as needed
            string mboxPath = "input.mbox";
            string pstPath = "output.pst";

            // Verify source MBOX file exists
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            // Ensure output directory exists
            string pstDir = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDir) && !Directory.Exists(pstDir))
            {
                try
                {
                    Directory.CreateDirectory(pstDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create directory '{pstDir}': {ex.Message}");
                    return;
                }
            }

            // Perform conversion inside a safe block
            PersonalStorage pst = null;
            try
            {
                pst = MailStorageConverter.MboxToPst(mboxPath, pstPath);
                Console.WriteLine($"Conversion succeeded. PST saved to '{pstPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Conversion failed: {ex.Message}");
            }
            finally
            {
                pst?.Dispose();
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
