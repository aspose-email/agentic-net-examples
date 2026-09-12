using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage;

// Author: Aspose.Email .NET example

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define source MBOX and target PST file paths
            string mboxFilePath = "input.mbox";
            string pstFilePath = "output.pst";

            // Ensure the source MBOX file exists; create a minimal placeholder if missing
            if (!File.Exists(mboxFilePath))
            {
                using (FileStream placeholder = File.Create(mboxFilePath))
                {
                    // Empty placeholder; no content needed for this example
                }
            }

            bool conversionSucceeded = false;

            try
            {
                // Perform the conversion from MBOX to PST
                MailStorageConverter.MboxToPst(mboxFilePath, pstFilePath);
                conversionSucceeded = true;
                Console.WriteLine("MBOX to PST conversion completed successfully.");
            }
            catch (Exception conversionEx)
            {
                Console.Error.WriteLine($"Conversion failed: {conversionEx.Message}");
            }

            // Cleanup: delete the PST file if conversion did not succeed
            if (!conversionSucceeded && File.Exists(pstFilePath))
            {
                try
                {
                    File.Delete(pstFilePath);
                    Console.WriteLine("Temporary PST file deleted due to conversion failure.");
                }
                catch (Exception deleteEx)
                {
                    Console.Error.WriteLine($"Failed to delete PST file: {deleteEx.Message}");
                }
            }
        }
        catch (Exception unexpectedEx)
        {
            Console.Error.WriteLine($"Unexpected error: {unexpectedEx.Message}");
        }
    }
}
