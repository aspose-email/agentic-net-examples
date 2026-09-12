using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for PST file and the file to be added
            const string pstPath = "MyArchive.pst";
            const string fileToAddPath = "SampleDocument.txt";

            // Ensure the source file exists; create a minimal placeholder if missing
            if (!File.Exists(fileToAddPath))
            {
                try
                {
                    File.WriteAllText(fileToAddPath, "Placeholder content for PST import.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder file: {ex.Message}");
                    return;
                }
            }

            // Ensure the PST file exists; create a new PST if it does not
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create a new PST file with Unicode format
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Use the root folder as the target folder
                FolderInfo rootFolder = pst.RootFolder;

                // Add the file to the PST folder with the appropriate message class
                // "IPM.Note" is the standard message class for email items
                string entryId;
                try
                {
                    entryId = rootFolder.AddFile(fileToAddPath, "IPM.Note");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to add file to PST: {ex.Message}");
                    return;
                }

                Console.WriteLine($"File added to PST. Entry ID: {entryId}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
