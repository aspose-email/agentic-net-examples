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
            // Path to the PST file
            string pstPath = "storage.pst";

            // Create a placeholder PST file if it does not exist
            if (!File.Exists(pstPath))
            {
                PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                Console.WriteLine($"Placeholder PST file created at: {pstPath}");
            }

            // Open the PST file with write access (second argument = false for read/write)
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath, false))
            {
                // Process the root folder and all its subfolders recursively
                ProcessFolder(pst, pst.RootFolder);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Recursively processes a folder, deleting messages that match a specific condition
    private static void ProcessFolder(PersonalStorage pst, FolderInfo folder)
    {
        try
        {
            // Enumerate all messages in the current folder
            foreach (MessageInfo messageInfo in folder.EnumerateMessages())
            {
                // Example condition: delete messages whose subject contains "[DeleteMe]"
                if (!string.IsNullOrEmpty(messageInfo.Subject) && messageInfo.Subject.Contains("[DeleteMe]"))
                {
                    // Convert EntryId to string if necessary
                    string entryId = messageInfo.EntryIdString ??
                                     BitConverter.ToString(messageInfo.EntryId).Replace("-", string.Empty);

                    // Delete the message by its entry ID
                    pst.DeleteItem(entryId);
                    Console.WriteLine($"Deleted message: {messageInfo.Subject}");
                }
            }

            // Recursively process each subfolder
            foreach (FolderInfo subFolder in folder.GetSubFolders())
            {
                ProcessFolder(pst, subFolder);
            }
        }
        catch (Exception ex)
        {
            // Log folder‑level errors but continue processing other folders
            Console.Error.WriteLine($"Folder processing error ({folder.DisplayName}): {ex.Message}");
        }
    }
}
