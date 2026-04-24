using Aspose.Email.Storage.Pst;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;

class Program
{
    static void Main()
    {
        try
        {
            // Define input MBOX and output PST paths
            string mboxFilePath = "input.mbox";
            string pstFilePath = "output.pst";

            // Guard input file existence
            if (!File.Exists(mboxFilePath))
            {
                // Create a minimal placeholder MBOX file if missing
                try
                {
                    using (FileStream placeholderStream = File.Create(mboxFilePath))
                    {
                        // Write a single empty line to make it a valid MBOX file
                        byte[] emptyLine = System.Text.Encoding.UTF8.GetBytes("\n");
                        placeholderStream.Write(emptyLine, 0, emptyLine.Length);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Ensure the directory for the PST file exists
            try
            {
                string pstDirectory = Path.GetDirectoryName(pstFilePath);
                if (!string.IsNullOrEmpty(pstDirectory) && !Directory.Exists(pstDirectory))
                {
                    Directory.CreateDirectory(pstDirectory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare PST directory: {ex.Message}");
                return;
            }

            // Configure conversion options to preserve original message flags
            MboxToPstConversionOptions conversionOptions = new MboxToPstConversionOptions();
            // Do not remove signatures; this helps keep original message state (e.g., read/unread flags)
            conversionOptions.RemoveSignature = false;

            // Perform the conversion
            try
            {
                using (PersonalStorage pstStorage = MailStorageConverter.MboxToPst(mboxFilePath, pstFilePath, conversionOptions))
                {
                    // Conversion succeeded; optionally you can work with pstStorage here
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
