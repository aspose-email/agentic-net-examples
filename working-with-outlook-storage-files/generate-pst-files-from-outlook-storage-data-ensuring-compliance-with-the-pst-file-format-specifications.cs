using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        // Author: Aspose.Email example - Convert MBOX to PST with safety checks
        string inputMboxPath = "input.mbox";
        string outputPstPath = "output.pst";

        // Ensure input file exists; create an empty placeholder if missing
        if (!File.Exists(inputMboxPath))
        {
            try
            {
                File.WriteAllText(inputMboxPath, string.Empty);
                Console.WriteLine($"Created placeholder MBOX file at '{inputMboxPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                return;
            }
        }

        // Ensure output directory exists
        string outputDir = Path.GetDirectoryName(outputPstPath);
        if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
        {
            try
            {
                Directory.CreateDirectory(outputDir);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create output directory '{outputDir}': {ex.Message}");
                return;
            }
        }

        // Perform conversion inside a try/catch block
        try
        {
            // Convert the MBOX storage to PST. The method returns a PersonalStorage instance.
            using (PersonalStorage pst = MailStorageConverter.MboxToPst(inputMboxPath, outputPstPath))
            {
                // The PST file is now created at outputPstPath.
                Console.WriteLine($"Conversion successful. PST file saved to '{outputPstPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Conversion failed: {ex.Message}");
        }
    }
}
