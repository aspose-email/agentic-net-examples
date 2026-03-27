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
            // Paths for the source MBOX file and the target PST file.
            string mboxPath = "input.mbox";
            string pstPath = "output.pst";

            // Option to remove signatures during conversion.
            bool removeSignature = true;

            // Ensure the source MBOX file exists; create a minimal placeholder if it does not.
            if (!File.Exists(mboxPath))
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(mboxPath))
                    {
                        writer.WriteLine("From - Mon Jan 01 00:00:00 2020");
                        writer.WriteLine("Subject: Placeholder Message");
                        writer.WriteLine();
                        writer.WriteLine("This is a placeholder email generated because the input MBOX file was missing.");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Ensure the directory for the PST file exists.
            try
            {
                string pstDirectory = Path.GetDirectoryName(pstPath);
                if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
                {
                    Directory.CreateDirectory(pstDirectory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare PST output directory: {ex.Message}");
                return;
            }

            // Configure conversion options.
            MboxToPstConversionOptions options = new MboxToPstConversionOptions
            {
                RemoveSignature = removeSignature
            };

            // Perform the conversion.
            using (PersonalStorage pst = MailStorageConverter.MboxToPst(mboxPath, pstPath, options))
            {
                // Conversion succeeded; the PST file is now available at pstPath.
                Console.WriteLine($"MBOX file '{mboxPath}' successfully converted to PST file '{pstPath}'.");
                Console.WriteLine($"Signature removal was {(removeSignature ? "enabled" : "disabled")}.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
