using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main(string[] args)
    {
        // Top‑level exception guard
        try
        {
            // Paths can be changed as needed
            string mboxPath = "input.mbox";
            string pstPath = "output.pst";

            // Guard file I/O: ensure the source MBOX exists
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"MBOX file not found: {mboxPath}");
                // Create a minimal placeholder MBOX to keep the sample runnable
                try
                {
                    File.WriteAllText(mboxPath, string.Empty);
                    Console.WriteLine($"Created empty placeholder MBOX at {mboxPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX: {ex.Message}");
                    return;
                }
            }

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(pstPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                try
                {
                    Directory.CreateDirectory(outputDir);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                    return;
                }
            }

            // Configure conversion options
            MboxToPstConversionOptions options = new MboxToPstConversionOptions
            {
                // Example option: strip signatures from messages
                RemoveSignature = true,
                // Example handler: add a custom header to each message before it is stored
                MessageHandler = (MailMessage msg) =>
                {
                    msg.Headers.Add("X-Converted-By", "Aspose.Email");
                }
            };

            // Perform the conversion; the returned PersonalStorage must be disposed
            using (PersonalStorage pst = MailStorageConverter.MboxToPst(mboxPath, pstPath, options))
            {
                Console.WriteLine($"MBOX to PST conversion succeeded. PST saved at: {pstPath}");
            }
        }
        catch (Exception ex)
        {
            // Global exception handling
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
