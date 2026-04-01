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

            // Ensure the PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                string directory = Path.GetDirectoryName(pstPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                try
                {
                    // Create an empty Unicode PST file
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Placeholder PST created at: {pstPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Process the root folder
                ProcessFolder(pst, pst.RootFolder);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Recursively process a folder and its subfolders
    private static void ProcessFolder(PersonalStorage pst, FolderInfo folder)
    {
        try
        {
            // Enumerate messages in the current folder
            foreach (MessageInfo messageInfo in folder.EnumerateMessages())
            {
                try
                {
                    using (MapiMessage message = pst.ExtractMessage(messageInfo))
                    {
                        MapiItemType itemType = message.SupportedType;
                        Console.WriteLine($"EntryId: {messageInfo.EntryId} | Type: {itemType}");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing message {messageInfo.EntryId}: {ex.Message}");
                }
            }

            // Recursively process subfolders
            foreach (FolderInfo subFolder in folder.GetSubFolders())
            {
                ProcessFolder(pst, subFolder);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error processing folder {folder.DisplayName}: {ex.Message}");
        }
    }
}
