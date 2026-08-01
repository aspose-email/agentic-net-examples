using System;
using System.IO;
using Aspose.Email.Storage.Pst;
using Aspose.Email;

class Program
{
    static void Main()
    {
        try
        {
            const string pstPath = "storage.pst";

            // Ensure the PST file exists; create a minimal one if missing
            if (!File.Exists(pstPath))
            {
                // Create a new Unicode PST file
                PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                Console.WriteLine($"Created placeholder PST file at '{pstPath}'.");
            }

            // Open the PST file for read/write operations
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Delete a folder named "ObsoleteFolder" if it exists
                foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                {
                    if (string.Equals(folderInfo.DisplayName, "ObsoleteFolder", StringComparison.OrdinalIgnoreCase))
                    {
                        // Convert the entry ID (byte[]) to a Base64 string as required by DeleteItem
                        string entryIdString = Convert.ToBase64String(folderInfo.EntryId);
                        pst.DeleteItem(entryIdString);
                        Console.WriteLine($"Deleted folder: {folderInfo.DisplayName}");
                        break;
                    }
                }

                // Delete a message with a specific subject if it exists
                bool messageDeleted = false;
                foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                {
                    foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
                    {
                        if (string.Equals(messageInfo.Subject, "Old Newsletter", StringComparison.OrdinalIgnoreCase))
                        {
                            string entryIdString = Convert.ToBase64String(messageInfo.EntryId);
                            pst.DeleteItem(entryIdString);
                            Console.WriteLine($"Deleted message: {messageInfo.Subject}");
                            messageDeleted = true;
                            break;
                        }
                    }
                    if (messageDeleted) break;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
