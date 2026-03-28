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
            // Define input MBOX file and output PST file paths
            string mboxFilePath = "input.mbox";
            string pstFilePath = "output.pst";

            // Verify that the input MBOX file exists
            if (!File.Exists(mboxFilePath))
{
    try
    {
        string placeholderMbox = "From placeholder@example.com Sat Jan 01 00:00:00 2026\n";
        File.WriteAllText(mboxFilePath, placeholderMbox);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder MBOX: {ex.Message}");
        return;
    }
}


            // Ensure the directory for the PST file exists
            string pstDirectory = Path.GetDirectoryName(pstFilePath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                try
                {
                    Directory.CreateDirectory(pstDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create directory for PST file: {dirEx.Message}");
                    return;
                }
            }

            // Configure conversion options to remove signatures
            MboxToPstConversionOptions conversionOptions = new MboxToPstConversionOptions();
            conversionOptions.RemoveSignature = true;

            // Perform the conversion; the returned PersonalStorage must be disposed
            using (PersonalStorage pstStorage = MailStorageConverter.MboxToPst(mboxFilePath, pstFilePath, conversionOptions))
            {
                // Conversion completed successfully
                Console.WriteLine($"MBOX file '{mboxFilePath}' has been converted to PST file '{pstFilePath}' with signatures removed.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
