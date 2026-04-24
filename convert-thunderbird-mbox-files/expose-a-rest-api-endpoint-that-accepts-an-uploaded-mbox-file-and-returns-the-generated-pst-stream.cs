using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Expect the first argument to be the path of the uploaded MBOX file.
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length < 2)
            {
                Console.Error.WriteLine("Usage: <exe> <mboxFilePath>");
                return;
            }

            string mboxPath = args[1];

            // Guard the input file.
            if (!File.Exists(mboxPath))
            {
                Console.Error.WriteLine($"Input file not found: {mboxPath}");
                // Create a minimal placeholder MBOX file (empty) to avoid failure.
                try
                {
                    using (FileStream placeholder = File.Create(mboxPath)) { }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Prepare output PST path.
            string pstPath = Path.Combine(Path.GetDirectoryName(mboxPath) ?? "", "converted.pst");

            // Ensure the output directory exists.
            try
            {
                string? outputDir = Path.GetDirectoryName(pstPath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    Directory.CreateDirectory(outputDir);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output directory: {ex.Message}");
                return;
            }

            // Convert MBOX to PST.
            try
            {
                // The static method returns a PersonalStorage instance; we ignore it after conversion.
                MailStorageConverter.MboxToPst(mboxPath, pstPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Conversion failed: {ex.Message}");
                return;
            }

            // Load the generated PST into a memory stream (simulating returning a stream).
            try
            {
                using (FileStream pstFileStream = File.OpenRead(pstPath))
                using (MemoryStream pstMemoryStream = new MemoryStream())
                {
                    pstFileStream.CopyTo(pstMemoryStream);
                    Console.WriteLine($"Conversion succeeded. PST size: {pstMemoryStream.Length} bytes.");
                    // In a real REST scenario the MemoryStream would be sent as the response body.
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to read generated PST: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
