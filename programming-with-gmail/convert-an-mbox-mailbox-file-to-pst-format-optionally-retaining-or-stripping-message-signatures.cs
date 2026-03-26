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
            string mboxFilePath = "input.mbox";
            string pstFilePath = "output.pst";

            // Verify input MBOX file exists
            if (!File.Exists(mboxFilePath))
            {
                Console.Error.WriteLine($"Input MBOX file not found: {mboxFilePath}");
                return;
            }

            // Ensure output directory exists
            string pstDirectory = Path.GetDirectoryName(pstFilePath);
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

            // Set conversion options (e.g., remove signatures)
            MboxToPstConversionOptions options = new MboxToPstConversionOptions();
            options.RemoveSignature = true; // Set to false to retain signatures

            // Perform conversion
            using (PersonalStorage pstStorage = MailStorageConverter.MboxToPst(mboxFilePath, pstFilePath, options))
            {
                // Conversion succeeded; pstStorage will be disposed automatically
                Console.WriteLine("MBOX to PST conversion completed successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}