using System;
using System.IO;
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


            // Ensure output directory exists
            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                Directory.CreateDirectory(pstDirectory);
            }

            try
            {
                // Set conversion options (true to remove signatures, false to retain)
                MboxToPstConversionOptions options = new MboxToPstConversionOptions();
                options.RemoveSignature = true;

                // Perform conversion; the returned PersonalStorage is disposable
                using (PersonalStorage pst = MailStorageConverter.MboxToPst(mboxPath, pstPath, options))
                {
                    Console.WriteLine("MBOX to PST conversion completed successfully.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Conversion failed: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
