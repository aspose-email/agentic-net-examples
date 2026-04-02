using Aspose.Email;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            // Paths for the source PST and the resulting PST after deletions
            string sourcePstPath = "sample.pst";
            string resultPstPath = "sample_modified.pst";

            // Ensure the source PST exists; if not, create an empty placeholder PST
            if (!File.Exists(sourcePstPath))
            {
                try
                {
                    // Create an empty Unicode PST file
                    using (PersonalStorage placeholder = PersonalStorage.Create(sourcePstPath, FileFormatVersion.Unicode))
                    {
                        // Optionally, create a default Inbox folder
                        placeholder.CreatePredefinedFolder("Inbox", StandardIpmFolder.Inbox);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Ensure the directory for the result PST exists
            try
            {
                string resultDirectory = Path.GetDirectoryName(resultPstPath);
                if (!string.IsNullOrEmpty(resultDirectory) && !Directory.Exists(resultDirectory))
                {
                    Directory.CreateDirectory(resultDirectory);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error preparing output directory: {ex.Message}");
                return;
            }

            // Open the source PST for read/write operations
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(sourcePstPath))
                {
                    if (!pst.CanWrite)
                    {
                        Console.Error.WriteLine("The PST file is read‑only and cannot be modified.");
                        return;
                    }

                    // Recursively delete all messages from the root folder and its subfolders
                    DeleteAllMessagesInFolder(pst.RootFolder);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing PST file: {ex.Message}");
                return;
            }

            // Copy the modified PST to the result path
            try
            {
                File.Copy(sourcePstPath, resultPstPath, true);
                Console.WriteLine($"All messages have been removed. Modified PST saved to '{resultPstPath}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error saving modified PST: {ex.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Deletes all messages within the specified folder and processes its subfolders
    private static void DeleteAllMessagesInFolder(FolderInfo folder)
    {
        // Collect entry IDs of all messages in the current folder
        List<string> messageEntryIds = new List<string>();
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            if (!string.IsNullOrEmpty(messageInfo.EntryIdString))
            {
                messageEntryIds.Add(messageInfo.EntryIdString);
            }
        }

        // Delete the collected messages in bulk, if any exist
        if (messageEntryIds.Count > 0)
        {
            try
            {
                folder.DeleteChildItems(messageEntryIds);
                Console.WriteLine($"Deleted {messageEntryIds.Count} messages from folder '{folder.DisplayName}'.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error deleting messages from folder '{folder.DisplayName}': {ex.Message}");
            }
        }

        // Recursively process subfolders
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            DeleteAllMessagesInFolder(subFolder);
        }
    }
}
