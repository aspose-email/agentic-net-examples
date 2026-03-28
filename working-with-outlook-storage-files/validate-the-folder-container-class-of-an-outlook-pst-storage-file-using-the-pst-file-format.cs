using System;
using System.IO;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string pstPath = "sample.pst";

            // Ensure the PST file exists; create a minimal placeholder if it does not.
            if (!File.Exists(pstPath))
            {
                try
                {
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Created placeholder PST at {pstPath}");
                }
                catch (Exception createEx)
                {
                    Console.Error.WriteLine($"Error creating placeholder PST: {createEx.Message}");
                    return;
                }
            }

            // Open the PST file and validate folder container classes.
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    ValidateFolderContainerClass(pst.RootFolder);
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

    private static void ValidateFolderContainerClass(FolderInfo folder)
    {
        // Output the folder's display name and its container class.
        Console.WriteLine($"Folder: {folder.DisplayName}, ContainerClass: {folder.ContainerClass}");

        // Recursively validate subfolders.
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            ValidateFolderContainerClass(subFolder);
        }
    }
}
