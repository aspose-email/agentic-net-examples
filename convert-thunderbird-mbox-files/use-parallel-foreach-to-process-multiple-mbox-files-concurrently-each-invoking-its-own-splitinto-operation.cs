using Aspose.Email;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Aspose.Email.Storage.Mbox;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define the list of MBOX files to process.
            List<string> mboxFiles = new List<string>
            {
                "mailbox1.mbox",
                "mailbox2.mbox",
                "mailbox3.mbox"
            };

            // Define the size of each split part (e.g., 10 MB).
            long chunkSize = 10L * 1024L * 1024L; // 10 MB

            // Process each MBOX file in parallel.
            Parallel.ForEach(mboxFiles, mboxPath =>
            {
                try
                {
                    // Verify that the input MBOX file exists.
                    if (!File.Exists(mboxPath))
                    {
                        Console.Error.WriteLine($"Input file not found: {mboxPath}");
                        return;
                    }

                    // Prepare the output directory for split parts.
                    string outputDir = Path.Combine("output", Path.GetFileNameWithoutExtension(mboxPath));
                    if (!Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }

                    // Create the MBOX reader with required load options.
                    using (MboxStorageReader mboxReader = MboxStorageReader.CreateReader(mboxPath, new MboxLoadOptions()))
                    {
                        // Perform the split operation.
                        mboxReader.SplitInto(chunkSize, outputDir);
                    }

                    Console.WriteLine($"Successfully split '{mboxPath}' into parts at '{outputDir}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing '{mboxPath}': {ex.Message}");
                }
            });
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unhandled exception: {ex.Message}");
        }
    }
}
