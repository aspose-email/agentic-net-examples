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
            // Paths for input PST and output folder
            string inputPstPath = "input.pst";
            string outputFolderPath = "output_parts";

            // Desired chunk size (e.g., 10 MB)
            long chunkSize = 10L * 1024L * 1024L;

            // Ensure the output directory exists
            if (!Directory.Exists(outputFolderPath))
            {
                Directory.CreateDirectory(outputFolderPath);
            }

            // Verify the input PST file; create a minimal placeholder if it does not exist
            if (!File.Exists(inputPstPath))
            {
                Console.Error.WriteLine($"Input PST not found. Creating placeholder at '{inputPstPath}'.");
                using (PersonalStorage placeholder = PersonalStorage.Create(inputPstPath, FileFormatVersion.Unicode))
                {
                    // Create a default Inbox folder to keep the PST valid
                    placeholder.CreatePredefinedFolder("Inbox", StandardIpmFolder.Inbox);
                }
            }

            // Open the PST file and split it into smaller PST parts
            using (PersonalStorage pst = PersonalStorage.FromFile(inputPstPath))
            {
                pst.SplitInto(chunkSize, outputFolderPath);
                Console.WriteLine($"PST split completed. Parts are stored in '{outputFolderPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
