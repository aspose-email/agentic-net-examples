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
            // Paths to the source PST (to be merged) and the target Outlook PST.
            string sourcePstPath = "source.pst";
            string targetPstPath = "target.pst";

            // Verify source PST exists.
            if (!File.Exists(sourcePstPath))
            {
                Console.Error.WriteLine($"Error: Source PST file not found – {sourcePstPath}");
                return;
            }

            // Ensure target PST exists; create a minimal Unicode PST if it does not.
            if (!File.Exists(targetPstPath))
            {
                try
                {
                    PersonalStorage.Create(targetPstPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating target PST: {ex.Message}");
                    return;
                }
            }

            // Open source PST for reading.
            using (PersonalStorage sourcePst = PersonalStorage.FromFile(sourcePstPath))
            // Open target PST for read/write.
            using (PersonalStorage targetPst = PersonalStorage.FromFile(targetPstPath))
            {
                // Iterate through each top‑level folder in the source PST.
                foreach (FolderInfo sourceFolder in sourcePst.RootFolder.GetSubFolders())
                {
                    // Try to locate a folder with the same name in the target PST.
                    FolderInfo targetFolder;
                    try
                    {
                        targetFolder = targetPst.RootFolder.GetSubFolder(sourceFolder.DisplayName);
                    }
                    catch (Exception)
                    {
                        // Folder does not exist; create it under the target root.
                        targetFolder = targetPst.RootFolder.AddSubFolder(sourceFolder.DisplayName);
                    }

                    // Merge the source folder into the target folder.
                    try
                    {
                        targetFolder.MergeWith(sourceFolder);
                    }
                    catch (Exception mergeEx)
                    {
                        Console.Error.WriteLine($"Error merging folder '{sourceFolder.DisplayName}': {mergeEx.Message}");
                        // Continue with next folder.
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
