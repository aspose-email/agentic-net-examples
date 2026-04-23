using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string mboxPath = "input.mbox";
            string pstPath = "output.pst";

            // Ensure input MBOX file exists
            if (!File.Exists(mboxPath))
            {
                try
                {
                    File.WriteAllText(mboxPath, string.Empty);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MBOX file: {ex.Message}");
                    return;
                }
            }

            // Convert MBOX to PST
            PersonalStorage pstStorage;
            try
            {
                pstStorage = MailStorageConverter.MboxToPst(mboxPath, pstPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Conversion failed: {ex.Message}");
                return;
            }

            // Open the created PST for further processing
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                try
                {
                    RenameFoldersRecursively(pst.RootFolder);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Folder renaming failed: {ex.Message}");
                    return;
                }
            }

            Console.WriteLine("PST conversion and folder renaming completed successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    private static void RenameFoldersRecursively(FolderInfo folder)
    {
        // Rename current folder (skip root folder which has no display name)
        if (!string.IsNullOrEmpty(folder.DisplayName))
        {
            string newName = GetRenamedFolderName(folder.DisplayName);
            try
            {
                folder.ChangeDisplayName(newName);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to rename folder '{folder.DisplayName}': {ex.Message}");
            }
        }

        // Process subfolders
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            RenameFoldersRecursively(subFolder);
        }
    }

    private static string GetRenamedFolderName(string originalName)
    {
        // Example naming convention: prepend "Renamed_"
        return $"Renamed_{originalName}";
    }
}
