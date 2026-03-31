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
            // Define input and output paths
            string mboxPath = "input.mbox";
            string pstPath = "output.pst";

            // Guard against missing input file
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
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }
            }

            // Perform the conversion
            using (PersonalStorage pstStorage = MailStorageConverter.MboxToPst(mboxPath, pstPath))
            {
                // Conversion succeeded; pstStorage can be used further if needed
                Console.WriteLine($"MBOX file '{mboxPath}' successfully converted to PST '{pstPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
