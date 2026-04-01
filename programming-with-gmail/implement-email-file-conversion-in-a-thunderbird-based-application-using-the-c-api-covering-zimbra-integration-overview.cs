using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Define input and output file paths
            string mboxFilePath = "input.mbox";
            string pstFilePath = "output.pst";

            // Ensure the directory for the input file exists
            string mboxDirectory = Path.GetDirectoryName(mboxFilePath);
            if (!string.IsNullOrEmpty(mboxDirectory) && !Directory.Exists(mboxDirectory))
            {
                Directory.CreateDirectory(mboxDirectory);
            }

            // Ensure the directory for the output file exists
            string pstDirectory = Path.GetDirectoryName(pstFilePath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                Directory.CreateDirectory(pstDirectory);
            }

            // Guard file I/O: create a minimal placeholder if the MBOX file is missing
            if (!File.Exists(mboxFilePath))
            {
                try
                {
                    File.WriteAllText(mboxFilePath, string.Empty);
                }
                catch (Exception ioEx)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ioEx.Message}");
                    return;
                }
            }

            // Perform the conversion inside a try/catch to handle conversion errors
            try
            {
                // Convert MBOX to PST; the returned PersonalStorage must be disposed
                using (PersonalStorage pstStorage = MailStorageConverter.MboxToPst(mboxFilePath, pstFilePath))
                {
                    // Conversion succeeded; optionally, you can work with pstStorage here
                    Console.WriteLine($"Conversion completed successfully. PST saved to: {pstFilePath}");
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
        }
    }
}
