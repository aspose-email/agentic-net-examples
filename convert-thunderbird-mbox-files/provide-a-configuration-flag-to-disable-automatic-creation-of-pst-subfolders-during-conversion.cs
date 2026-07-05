using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

// Author: Aspose.Email example - MBOX to PST conversion with options

class Program
{
    static void Main()
    {
        try
        {
            // Input and output file paths
            string mboxPath = "input.mbox";
            string pstPath = "output.pst";

            // Ensure the input MBOX file exists; create an empty placeholder if missing
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

            // Ensure the directory for the PST file exists
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

            // Configure conversion options
            MboxToPstConversionOptions options = new MboxToPstConversionOptions();

            // Note: Aspose.Email does not provide a specific flag to disable automatic PST subfolder creation.
            // The conversion will create the target folder if it does not exist.

            // Perform the conversion
            MailStorageConverter.MboxToPst(mboxPath, pstPath, options);
            Console.WriteLine("MBOX to PST conversion completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
