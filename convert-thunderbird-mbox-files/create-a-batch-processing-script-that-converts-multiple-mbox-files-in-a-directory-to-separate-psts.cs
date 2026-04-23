using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define input and output directories
            string inputDirectory = "MboxFiles";
            string outputDirectory = "PstFiles";

            // Ensure input directory exists
            if (!Directory.Exists(inputDirectory))
            {
                Console.Error.WriteLine($"Input directory '{inputDirectory}' does not exist.");
                return;
            }

            // Ensure output directory exists or create it
            if (!Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create output directory: {dirEx.Message}");
                    return;
                }
            }

            // Get all .mbox files in the input directory
            string[] mboxFiles;
            try
            {
                mboxFiles = Directory.GetFiles(inputDirectory, "*.mbox");
            }
            catch (Exception getFilesEx)
            {
                Console.Error.WriteLine($"Failed to enumerate .mbox files: {getFilesEx.Message}");
                return;
            }

            // Process each MBOX file
            foreach (string mboxPath in mboxFiles)
            {
                // Verify the MBOX file exists; if not, create a minimal placeholder
                if (!File.Exists(mboxPath))
                {
                    try
                    {
                        using (FileStream placeholderStream = File.Create(mboxPath))
                        using (StreamWriter writer = new StreamWriter(placeholderStream))
                        {
                            writer.WriteLine("From - Mon Jan 01 00:00:00 2020");
                            writer.WriteLine("Subject: Placeholder");
                            writer.WriteLine();
                            writer.WriteLine("This is a placeholder message.");
                        }
                    }
                    catch (Exception placeholderEx)
                    {
                        Console.Error.WriteLine($"Failed to create placeholder for '{mboxPath}': {placeholderEx.Message}");
                        continue;
                    }
                }

                // Determine PST output path
                string mboxFileName = Path.GetFileNameWithoutExtension(mboxPath);
                string pstPath = Path.Combine(outputDirectory, mboxFileName + ".pst");

                // Convert MBOX to PST
                try
                {
                    using (PersonalStorage pst = MailStorageConverter.MboxToPst(mboxPath, pstPath))
                    {
                        // Conversion succeeded; optionally you can work with the PST here
                        Console.WriteLine($"Converted '{mboxPath}' to '{pstPath}'.");
                    }
                }
                catch (Exception convertEx)
                {
                    Console.Error.WriteLine($"Failed to convert '{mboxPath}' to PST: {convertEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
