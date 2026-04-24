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
            // Input and output paths
            string mboxPath = "input.mbox";
            string pstPath = "output.pst";

            // Ensure MBOX file exists; create a minimal placeholder if missing
            if (!File.Exists(mboxPath))
            {
                try
                {
                    using (var placeholder = File.Create(mboxPath)) { }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Ensure the directory for PST exists
            string pstDirectory = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
            {
                try
                {
                    Directory.CreateDirectory(pstDirectory);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST directory: {ex.Message}");
                    return;
                }
            }

            // Create PST file (or open if it already exists)
            PersonalStorage pst;
            if (File.Exists(pstPath))
            {
                try
                {
                    pst = PersonalStorage.FromFile(pstPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to open existing PST file: {ex.Message}");
                    return;
                }
            }
            else
            {
                try
                {
                    pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // Use using to ensure PST is disposed
            using (pst)
            {
                // Create MBOX reader
                MboxStorageReader mboxReader;
                try
                {
                    mboxReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions());
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create MBOX reader: {ex.Message}");
                    return;
                }

                // Ensure reader is disposed after conversion
                using (mboxReader)
                {
                    // Prepare conversion options with a no‑op handler
                    var options = new MboxToPstConversionOptions
                    {
                        MessageHandler = message => { /* no additional processing */ }
                    };

                    // Perform conversion; specify the target folder name inside PST
                    try
                    {
                        MailStorageConverter.MboxToPst(mboxReader, pst, "ImportedMbox", options);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Conversion failed: {ex.Message}");
                        return;
                    }
                } // mboxReader disposed here
            } // pst disposed here

            Console.WriteLine("MBOX to PST conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
