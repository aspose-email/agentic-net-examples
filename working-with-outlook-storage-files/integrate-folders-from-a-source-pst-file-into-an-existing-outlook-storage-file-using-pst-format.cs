using System;
using System.IO;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Paths to the source PST (to copy from) and the destination PST (to copy into)
            string sourcePstPath = "source.pst";
            string destinationPstPath = "destination.pst";

            // Verify source PST exists
            if (!File.Exists(sourcePstPath))
            {
                Console.Error.WriteLine($"Error: Source PST file not found – {sourcePstPath}");
                return;
            }

            // Ensure destination PST exists; create a minimal PST if it does not
            if (!File.Exists(destinationPstPath))
            {
                try
                {
                    // Create a new Unicode PST file
                    PersonalStorage.Create(destinationPstPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error: Unable to create destination PST – {ex.Message}");
                    return;
                }
            }

            // Open both PST files
            using (PersonalStorage sourcePst = PersonalStorage.FromFile(sourcePstPath))
            using (PersonalStorage destinationPst = PersonalStorage.FromFile(destinationPstPath))
            {
                // Get root folders
                FolderInfo sourceRoot = sourcePst.RootFolder;
                FolderInfo destinationRoot = destinationPst.RootFolder;

                // Iterate through each top‑level folder in the source PST
                foreach (FolderInfo sourceFolder in sourceRoot.GetSubFolders())
                {
                    // Create a folder with the same display name in the destination PST
                    FolderInfo destFolder;
                    try
                    {
                        destFolder = destinationRoot.AddSubFolder(sourceFolder.DisplayName);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Warning: Could not add folder '{sourceFolder.DisplayName}' – {ex.Message}");
                        continue;
                    }

                    // Merge the source folder into the newly created destination folder
                    try
                    {
                        destFolder.MergeWith(sourceFolder);
                        Console.WriteLine($"Merged folder '{sourceFolder.DisplayName}' successfully.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error: Failed to merge folder '{sourceFolder.DisplayName}' – {ex.Message}");
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
