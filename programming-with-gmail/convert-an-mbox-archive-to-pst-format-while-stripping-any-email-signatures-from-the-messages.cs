using Aspose.Email.Storage.Pst;
using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage;

class Program
{
    static void Main()
    {
        try
        {
            // Input and output file paths
            string mboxPath = "input.mbox";
            string pstPath = "output.pst";

            // Verify input MBOX file exists
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Input MBOX file not found: {mboxPath}");
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
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Set conversion options to remove signatures
            MboxToPstConversionOptions options = new MboxToPstConversionOptions
            {
                RemoveSignature = true
            };

            // Perform conversion
            try
            {
                using (PersonalStorage pst = MailStorageConverter.MboxToPst(mboxPath, pstPath, options))
                {
                    // Conversion completed successfully
                    Console.WriteLine($"MBOX file '{mboxPath}' has been converted to PST '{pstPath}' with signatures removed.");
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
