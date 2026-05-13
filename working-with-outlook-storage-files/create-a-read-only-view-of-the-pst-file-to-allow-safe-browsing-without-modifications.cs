using Aspose.Email;
using Aspose.Email.Storage.Pst;
using System;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";

            // Ensure a PST file exists; create a minimal one if missing
            if (!File.Exists(pstPath))
            {
                // Create a new empty PST file (Unicode format)
                PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                Console.WriteLine($"Created placeholder PST file at: {pstPath}");
            }

            // Open PST in read‑only mode
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath, true))
            {
                // Display total items count
                int totalItems = pst.Store.GetTotalItemsCount();
                Console.WriteLine($"Total items count: {totalItems}");

                // Iterate through each subfolder of the root folder
                foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                {
                    Console.WriteLine($"Folder: {folderInfo.DisplayName}");
                    Console.WriteLine($"Total items: {folderInfo.ContentCount}");
                    Console.WriteLine($"Total unread items: {folderInfo.ContentUnreadCount}");

                    // Enumerate messages in the current folder
                    foreach (MessageInfo messageInfo in folderInfo.EnumerateMessages())
                    {
                        Console.WriteLine($"Subject: {messageInfo.Subject}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
