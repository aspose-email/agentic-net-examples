using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

class Program
{
    // Author: Aspose.Email .NET sample for MBOX to PST conversion
    static void Main(string[] args)
    {
        try
        {
            // Input and output paths (can be overridden via command‑line arguments)
            string mboxPath = args.Length > 0 ? args[0] : "input.mbox";
            string pstPath = args.Length > 1 ? args[1] : "output.pst";

            // Guard: ensure source MBOX exists; create an empty placeholder if missing
            if (!File.Exists(mboxPath))
            {
                try
                {
                    File.WriteAllText(mboxPath, string.Empty);
                    Console.WriteLine($"Placeholder MBOX created at '{mboxPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX: {ex.Message}");
                    return;
                }
            }

            // Guard: ensure destination directory exists
            try
            {
                string pstDir = Path.GetDirectoryName(pstPath);
                if (!string.IsNullOrEmpty(pstDir) && !Directory.Exists(pstDir))
                {
                    Directory.CreateDirectory(pstDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare PST directory: {ex.Message}");
                return;
            }

            // Perform conversion inside a try/catch to handle library errors
            try
            {
                // MailStorageConverter.MboxToPst returns a PersonalStorage instance which must be disposed
                using (PersonalStorage pst = MailStorageConverter.MboxToPst(mboxPath, pstPath))
                {
                    // Conversion succeeded; additional processing can be added here if needed
                    Console.WriteLine($"Successfully converted '{mboxPath}' to PST '{pstPath}'.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Conversion failed: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
