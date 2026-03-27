using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define source MBOX file and destination PST file paths
            string mboxFilePath = "input.mbox";
            string pstFilePath = "output.pst";

            // Verify that the source MBOX file exists
            if (!File.Exists(mboxFilePath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxFilePath}");
                return;
            }

            // Ensure the directory for the PST file exists
            string pstDirectory = Path.GetDirectoryName(pstFilePath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                Directory.CreateDirectory(pstDirectory);
            }

            // Set conversion options (e.g., remove signatures)
            MboxToPstConversionOptions options = new MboxToPstConversionOptions();
            options.RemoveSignature = true; // Set to false to retain signatures

            // Perform the conversion; the returned PersonalStorage must be disposed
            using (PersonalStorage pst = MailStorageConverter.MboxToPst(mboxFilePath, pstFilePath, options))
            {
                Console.WriteLine($"MBOX file '{mboxFilePath}' successfully converted to PST file '{pstFilePath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
