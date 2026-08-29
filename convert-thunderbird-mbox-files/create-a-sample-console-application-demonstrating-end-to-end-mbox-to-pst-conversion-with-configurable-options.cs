using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            // Define input MBOX and output PST file paths
            string mboxFilePath = "input.mbox";
            string pstFilePath = "output.pst";

            // Ensure the input MBOX file exists; create a minimal placeholder if missing
            if (!File.Exists(mboxFilePath))
            {
                try
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(mboxFilePath) ?? ".");
                    // Minimal MBOX content (single empty message)
                    File.WriteAllText(mboxFilePath, "From - Mon Jan 01 00:00:00 2020\r\n\r\n");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Ensure the directory for the PST file exists
            try
            {
                string pstDirectory = Path.GetDirectoryName(pstFilePath) ?? ".";
                Directory.CreateDirectory(pstDirectory);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create PST directory: {ex.Message}");
                return;
            }

            // Configure conversion options (default options used here)
            MboxToPstConversionOptions conversionOptions = new MboxToPstConversionOptions();

            // Perform the conversion inside a using block to ensure the PST storage is disposed
            try
            {
                using (PersonalStorage pstStorage = MailStorageConverter.MboxToPst(mboxFilePath, pstFilePath, conversionOptions))
                {
                    // Conversion succeeded; the PST file is now created at pstFilePath
                    Console.WriteLine($"MBOX file '{mboxFilePath}' successfully converted to PST file '{pstFilePath}'.");
                }
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
