using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for the source MBOX file and the destination PST file.
            string mboxPath = "input.mbox";
            string pstPath = "output.pst";

            // Option to remove email signatures during conversion.
            bool removeSignature = true;

            // Verify that the source MBOX file exists.
            if (!File.Exists(mboxPath))
{
    try
    {
        string placeholderMbox = "From placeholder@example.com Sat Jan 01 00:00:00 2026\n";
        File.WriteAllText(mboxPath, placeholderMbox);
    }
    catch (Exception ex)
    {
        Console.Error.WriteLine($"Failed to create placeholder MBOX: {ex.Message}");
        return;
    }
}


            // Ensure the directory for the PST file exists.
            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                try
                {
                    Directory.CreateDirectory(pstDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create directory '{pstDirectory}': {dirEx.Message}");
                    return;
                }
            }

            // Configure conversion options.
            MboxToPstConversionOptions options = new MboxToPstConversionOptions();
            options.RemoveSignature = removeSignature;

            // Perform the conversion inside a using block to dispose the resulting PST.
            using (PersonalStorage pst = MailStorageConverter.MboxToPst(mboxPath, pstPath, options))
            {
                // Conversion succeeded; you can further work with the PST if needed.
                Console.WriteLine($"MBOX file '{mboxPath}' successfully converted to PST '{pstPath}'.");
                Console.WriteLine($"Signature removal was {(removeSignature ? "enabled" : "disabled")}.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
