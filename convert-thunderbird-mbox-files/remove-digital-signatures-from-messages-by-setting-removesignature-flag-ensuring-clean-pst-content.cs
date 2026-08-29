using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

namespace RemoveSignatureExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Input MBOX file and output PST file paths
                string mboxPath = "input.mbox";
                string pstPath = "output.pst";

                // Verify input file exists
                if (!File.Exists(mboxPath))
                {
                    Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                    return;
                }

                // Ensure the output directory exists
                string pstDirectory = Path.GetDirectoryName(pstPath);
                if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
                {
                    Directory.CreateDirectory(pstDirectory);
                }

                // Configure conversion options to remove digital signatures
                MboxToPstConversionOptions options = new MboxToPstConversionOptions
                {
                    RemoveSignature = true
                };

                // Perform the conversion
                using (PersonalStorage pst = MailStorageConverter.MboxToPst(mboxPath, pstPath, options))
                {
                    // Conversion completed; PST is saved at pstPath
                }

                Console.WriteLine("Conversion completed successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
