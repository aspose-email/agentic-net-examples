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
            const string ostPath = "sample.ost";

            // Ensure the OST file exists; create a minimal placeholder if missing.
            if (!File.Exists(ostPath))
            {
                try
                {
                    // Create an empty Unicode PST (used here as a placeholder for OST).
                    using (PersonalStorage placeholder = PersonalStorage.Create(ostPath, FileFormatVersion.Unicode))
                    {
                        // No additional setup required.
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder OST file: {ex.Message}");
                    return;
                }
            }

            // Open the OST file.
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(ostPath))
                {
                    // Enumerate all subfolders starting from the root.
                    EnumerateFolder(pst.RootFolder, string.Empty);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to open or process the OST file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Recursively enumerate folders and write their display names.
    private static void EnumerateFolder(FolderInfo folder, string indent)
    {
        try
        {
            Console.WriteLine($"{indent}Folder: {folder.DisplayName}");

            // If the folder has subfolders, enumerate them.
            foreach (FolderInfo subFolder in folder.GetSubFolders())
            {
                EnumerateFolder(subFolder, indent + "  ");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error enumerating folder '{folder?.DisplayName}': {ex.Message}");
        }
    }
}
