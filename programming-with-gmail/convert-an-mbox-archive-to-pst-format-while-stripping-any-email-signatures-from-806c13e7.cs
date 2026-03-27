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
            string mboxPath = "input.mbox";
            string pstPath = "output.pst";

            // Verify input file exists
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                return;
            }

            // Ensure output directory exists
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
            using (PersonalStorage pst = MailStorageConverter.MboxToPst(mboxPath, pstPath, options))
            {
                // Conversion completed; pst will be disposed automatically
                Console.WriteLine("MBOX successfully converted to PST with signatures removed.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
