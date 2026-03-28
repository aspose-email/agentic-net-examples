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
            string pstPath = "sample.pst";

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
                    Console.Error.WriteLine($"Error: Unable to create directory '{pstDirectory}'. {ex.Message}");
                    return;
                }
            }

            // Create a new PST file if it does not exist
            if (!File.Exists(pstPath))
            {
                try
                {
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Created new PST file at '{pstPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: Unable to create PST file. {ex.Message}");
                    return;
                }
            }

            // Open the PST file
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Add a top‑level subfolder
                    FolderInfo folderA = pst.RootFolder.AddSubFolder("FolderA");
                    Console.WriteLine($"Created folder: {folderA.DisplayName}");

                    // Add a nested subfolder using hierarchy creation
                    FolderInfo folderB = pst.RootFolder.AddSubFolder(@"FolderA\FolderB", true);
                    Console.WriteLine($"Created nested folder: {folderB.DisplayName}");

                    // Add another independent subfolder
                    FolderInfo folderC = pst.RootFolder.AddSubFolder("FolderC");
                    Console.WriteLine($"Created folder: {folderC.DisplayName}");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: Unable to open or modify PST file. {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
