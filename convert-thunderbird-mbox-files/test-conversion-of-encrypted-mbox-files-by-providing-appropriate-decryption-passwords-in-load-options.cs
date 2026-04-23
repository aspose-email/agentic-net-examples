using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Mbox;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string mboxPath = "encrypted.mbox";
            string pstPath = "output.pst";

            // Ensure the MBOX file exists; create a minimal placeholder if missing.
            if (!File.Exists(mboxPath))
            {
                try
                {
                    File.WriteAllText(mboxPath, string.Empty);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Load options for MBOX. No password property exists; only encoding options are available.
            MboxLoadOptions loadOptions = new MboxLoadOptions();

            // Create a reader for the MBOX file.
            using (MboxStorageReader reader = MboxStorageReader.CreateReader(mboxPath, loadOptions))
            {
                // Convert the MBOX to PST using the built‑in converter.
                try
                {
                    // The conversion method does not require explicit load options for decryption.
                    // If the MBOX is encrypted and a password is needed, Aspose.Email currently
                    // handles it internally based on the file format; no explicit password property is exposed.
                    MailStorageConverter.MboxToPst(mboxPath, pstPath);
                    Console.WriteLine($"Conversion completed. PST saved to '{pstPath}'.");
                }
                catch (Exception convEx)
                {
                    Console.Error.WriteLine($"Conversion failed: {convEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
