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
            // Determine input and output paths (hard‑coded for example purposes)
            string mboxPath = "input.mbox";
            string pstPath = "output.pst";

            // Check for verbose flag
            bool verbose = false;
            foreach (string arg in args)
            {
                if (arg.Equals("--verbose", StringComparison.OrdinalIgnoreCase))
                {
                    verbose = true;
                    break;
                }
            }

            // Ensure the input MBOX file exists; create a minimal placeholder if missing
            if (!File.Exists(mboxPath))
            {
                if (verbose)
                    Console.WriteLine($"Input file \"{mboxPath}\" not found. Creating empty placeholder.");
                try
                {
                    using (FileStream placeholder = File.Create(mboxPath))
                    {
                        // No content needed for an empty MBOX
                    }
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

            // Define the mail handler to log each conversion step when verbose is enabled
            MailStorageConverter.MailHandler handler = null;
            if (verbose)
            {
                handler = delegate (MailMessage message)
                {
                    try
                    {
                        Console.WriteLine($"Converting message: Subject=\"{message.Subject}\" From=\"{message.From}\"");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Logging error: {ex.Message}");
                    }
                };
            }

            // Perform the conversion inside a guarded block
            try
            {
                if (verbose)
                    Console.WriteLine("Starting MBOX to PST conversion...");

                PersonalStorage pstStorage = MailStorageConverter.MboxToPst(mboxPath, pstPath, handler);

                if (verbose)
                    Console.WriteLine("Conversion completed successfully.");
                // Dispose the PST storage
                pstStorage.Dispose();
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
