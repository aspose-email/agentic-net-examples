using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Define paths
            string inputPstPath = "input.pst";
            string outputFolder = "output";

            // Ensure the output directory exists
            if (!Directory.Exists(outputFolder))
            {
                Directory.CreateDirectory(outputFolder);
            }

            // Create a minimal PST file if the input file is missing
            if (!File.Exists(inputPstPath))
            {
                try
                {
                    // Create a new Unicode PST file
                    using (PersonalStorage placeholderPst = PersonalStorage.Create(inputPstPath, FileFormatVersion.Unicode))
                    {
                        // Create a default Inbox folder inside the placeholder PST
                        placeholderPst.CreatePredefinedFolder("Inbox", StandardIpmFolder.Inbox);
                    }
                }
                catch (Exception createEx)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST file: {createEx.Message}");
                    return;
                }
            }

            // Open the PST file
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(inputPstPath))
                {
                    // Define the maximum size of each split part (e.g., 10 MB)
                    long chunkSize = 10L * 1024L * 1024L; // 10 megabytes

                    // Split the PST into smaller parts
                    pst.SplitInto(chunkSize, outputFolder);
                    
                    Console.WriteLine("PST split operation completed successfully.");
                }
            }
            catch (Exception pstEx)
            {
                Console.Error.WriteLine($"Error processing PST file: {pstEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
