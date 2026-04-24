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
            // Define input and output directories
            string inputDirectory = "MboxFiles";
            string outputDirectory = "PstFiles";

            // Ensure input directory exists
            try
            {
                if (!Directory.Exists(inputDirectory))
                {
                    Directory.CreateDirectory(inputDirectory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare input directory '{inputDirectory}': {ex.Message}");
                return;
            }

            // Ensure output directory exists
            try
            {
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory '{outputDirectory}': {ex.Message}");
                return;
            }

            // Get all .mbox files in the input directory
            string[] mboxFiles;
            try
            {
                mboxFiles = Directory.GetFiles(inputDirectory, "*.mbox");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to enumerate .mbox files: {ex.Message}");
                return;
            }

            // Process each MBOX file
            foreach (string mboxFilePath in mboxFiles)
            {
                // Verify the MBOX file exists; if not, create a minimal placeholder
                if (!File.Exists(mboxFilePath))
                {
                    try
                    {
                        string placeholderMessage = "From - Mon Jan 01 00:00:00 2000\r\nSubject: Placeholder\r\n\r\nThis is a placeholder message.\r\n";
                        File.WriteAllText(mboxFilePath, placeholderMessage);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder for missing file '{mboxFilePath}': {ex.Message}");
                        continue;
                    }
                }

                // Determine PST output path
                string pstFileName = Path.GetFileNameWithoutExtension(mboxFilePath) + ".pst";
                string pstFilePath = Path.Combine(outputDirectory, pstFileName);

                // Convert MBOX to PST
                try
                {
                    using (PersonalStorage pst = MailStorageConverter.MboxToPst(mboxFilePath, pstFilePath))
                    {
                        // Conversion succeeded; optionally you can work with 'pst' here
                    }

                    Console.WriteLine($"Converted '{mboxFilePath}' to '{pstFilePath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to convert '{mboxFilePath}' to PST: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
