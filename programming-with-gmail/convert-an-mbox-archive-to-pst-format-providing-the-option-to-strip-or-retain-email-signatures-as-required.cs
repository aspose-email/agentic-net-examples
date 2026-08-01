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
            // Input MBOX file path
            string mboxPath = "input.mbox";
            // Output PST file path
            string pstPath = "output.pst";

            // Ensure the input MBOX file exists; create an empty placeholder if missing
            if (!File.Exists(mboxPath))
            {
                try
                {
                    File.WriteAllText(mboxPath, string.Empty);
                    Console.WriteLine($"Created placeholder MBOX file at '{mboxPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
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

            // Configure conversion options
            MboxToPstConversionOptions options = new MboxToPstConversionOptions();
            // Set to true to strip signatures, false to retain them
            options.RemoveSignature = true;

            // Perform the conversion
            using (PersonalStorage pst = MailStorageConverter.MboxToPst(mboxPath, pstPath, options))
            {
                // The PST file is now created; additional processing can be done here if needed
                Console.WriteLine($"MBOX file '{mboxPath}' successfully converted to PST '{pstPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
