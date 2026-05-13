using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";

            // Ensure PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Get the root folder
                FolderInfo rootFolder = pst.RootFolder;

                // Create or retrieve target folders
                FolderInfo highFolder = GetOrCreateFolder(pst, "High");
                FolderInfo normalFolder = GetOrCreateFolder(pst, "Normal");
                FolderInfo lowFolder = GetOrCreateFolder(pst, "Low");

                // Enumerate all messages in the root folder
                foreach (MessageInfo messageInfo in rootFolder.EnumerateMessages())
                {
                    // Determine importance using MapiImportance
                    MapiImportance importance = messageInfo.Importance;

                    // Choose target folder based on importance
                    FolderInfo targetFolder = importance switch
                    {
                        MapiImportance.High => highFolder,
                        MapiImportance.Low => lowFolder,
                        _ => normalFolder,
                    };

                    try
                    {
                        // Move the message to the appropriate folder
                        pst.MoveItem(messageInfo, targetFolder);
                        Console.WriteLine($"Moved message '{messageInfo.Subject}' to folder '{targetFolder.DisplayName}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to move message '{messageInfo.Subject}': {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Helper to get an existing folder or create a new one if it does not exist
    private static FolderInfo GetOrCreateFolder(PersonalStorage pst, string folderName)
    {
        try
        {
            // Try to find the folder by name
            foreach (FolderInfo subFolder in pst.RootFolder.GetSubFolders())
            {
                if (string.Equals(subFolder.DisplayName, folderName, StringComparison.OrdinalIgnoreCase))
                {
                    return subFolder;
                }
            }

            // Folder not found; create a new one (using Unspecified type for custom folders)
            return pst.CreatePredefinedFolder(folderName, StandardIpmFolder.Unspecified);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error accessing or creating folder '{folderName}': {ex.Message}");
            throw;
        }
    }
}
