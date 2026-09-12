using Aspose.Email;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email.Storage.Pst;

namespace AsposeEmailPstBulkDelete
{
    class Program
    {
        static void Main()
        {
            try
            {
                const string pstPath = "storage.pst";

                // Ensure the PST file exists; create a minimal one if missing.
                if (!File.Exists(pstPath))
                {
                    // Create an empty PST file with Unicode format.
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Created placeholder PST file at '{pstPath}'.");
                }

                // Open the PST file.
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Recursively delete all messages in the PST.
                    DeleteMessagesInFolder(pst.RootFolder);
                }

                Console.WriteLine("Bulk deletion completed successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        // Recursively deletes all messages within the specified folder and its subfolders.
        private static void DeleteMessagesInFolder(FolderInfo folder)
        {
            // Collect entry IDs of all messages in the current folder.
            List<string> entryIds = new List<string>();
            foreach (MessageInfo messageInfo in folder.EnumerateMessages())
            {
                // Convert the byte[] EntryId to a Base64 string for deletion.
                entryIds.Add(Convert.ToBase64String(messageInfo.EntryId));
            }

            // Delete the collected messages in bulk, if any.
            if (entryIds.Count > 0)
            {
                folder.DeleteChildItems(entryIds.ToArray());
                Console.WriteLine($"Deleted {entryIds.Count} message(s) from folder '{folder.DisplayName}'.");
            }

            // Process subfolders recursively.
            foreach (FolderInfo subFolder in folder.GetSubFolders())
            {
                DeleteMessagesInFolder(subFolder);
            }
        }
    }
}
