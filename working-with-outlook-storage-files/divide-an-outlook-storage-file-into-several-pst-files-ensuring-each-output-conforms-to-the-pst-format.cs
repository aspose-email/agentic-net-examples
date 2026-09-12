using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input PST file path
            string inputPstPath = "storage.pst";

            // Output folder where split parts will be created
            string outputFolder = "SplitParts";

            // Approximate size of each split part (e.g., 10 MB)
            long chunkSize = 10L * 1024L * 1024L;

            // Prefix for split part file names
            string partFileNamePrefix = "part_";

            // Ensure the input PST file exists; create a minimal placeholder if missing
            if (!File.Exists(inputPstPath))
            {
                try
                {
                    using (PersonalStorage placeholder = PersonalStorage.Create(inputPstPath, FileFormatVersion.Unicode))
                    {
                        // Optionally create a root folder or leave empty
                    }
                    Console.WriteLine($"Placeholder PST created at '{inputPstPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Ensure the output directory exists
            try
            {
                if (!Directory.Exists(outputFolder))
                {
                    Directory.CreateDirectory(outputFolder);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to prepare output folder '{outputFolder}': {ex.Message}");
                return;
            }

            // Load the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(inputPstPath))
            {
                // Split the PST into smaller parts
                pst.SplitInto(chunkSize, partFileNamePrefix, outputFolder);
            }

            Console.WriteLine("PST split operation completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
