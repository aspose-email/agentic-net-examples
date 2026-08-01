using Aspose.Email.Storage.Pst;
using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input MBOX file path
            string mboxFilePath = "input.mbox";
            // Output PST file path
            string pstFilePath = "output.pst";
            // Set to true to remove signatures, false to retain them
            bool removeSignature = true;

            // Guard input file existence
            if (!File.Exists(mboxFilePath))
            {
                Console.Error.WriteLine($"Input MBOX file not found: {mboxFilePath}");
                return;
            }

            // Ensure output directory exists
            string pstDirectory = Path.GetDirectoryName(pstFilePath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                Directory.CreateDirectory(pstDirectory);
            }

            // Configure conversion options
            MboxToPstConversionOptions options = new MboxToPstConversionOptions();
            options.RemoveSignature = removeSignature;

            // Perform conversion
            PersonalStorage pstStore = MailStorageConverter.MboxToPst(mboxFilePath, pstFilePath, options);
            // Dispose the resulting PST storage
            pstStore.Dispose();

            Console.WriteLine("MBOX to PST conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
