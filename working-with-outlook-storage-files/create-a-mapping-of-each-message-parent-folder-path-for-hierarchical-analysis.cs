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

            // Create a minimal PST file if it does not exist
            if (!File.Exists(pstPath))
            {
                try
                {
                    using (PersonalStorage pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Root folder is created by default
                    }
                    Console.WriteLine($"Placeholder PST file created at: {pstPath}");
                }
                catch (Exception createEx)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST file: {createEx.Message}");
                    return;
                }
            }

            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                FolderInfo rootFolder = pst.RootFolder;
                ProcessFolder(rootFolder, rootFolder.DisplayName);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    private static void ProcessFolder(FolderInfo folder, string currentPath)
    {
        if (folder == null) return;

        try
        {
            foreach (MessageInfo messageInfo in folder.EnumerateMessages())
            {
                Console.WriteLine($"{messageInfo.Subject} -> {currentPath}");
            }

            foreach (FolderInfo subFolder in folder.GetSubFolders())
            {
                string subPath = Path.Combine(currentPath, subFolder.DisplayName);
                ProcessFolder(subFolder, subPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing folder '{folder?.DisplayName}': {ex.Message}");
        }
    }
}
