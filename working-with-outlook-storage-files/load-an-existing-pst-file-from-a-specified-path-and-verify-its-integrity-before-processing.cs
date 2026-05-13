using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Specify the PST file path
            string pstPath = "sample.pst";

            // Create a placeholder PST file if it does not exist
            if (!File.Exists(pstPath))
            {
                using (PersonalStorage pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                {
                    // Add a sample folder
                    FolderInfo inbox = pst.RootFolder.AddSubFolder("Inbox");

                    // Create a simple email message
                    MailMessage msg = new MailMessage(
                        "sender@example.com",
                        "receiver@example.com",
                        "Test Subject",
                        "This is a test message.");

                    // Add the message to the folder
                    inbox.AddMessage(MapiMessage.FromMailMessage(msg));
                }

                Console.WriteLine($"Placeholder PST file created at: {pstPath}");
            }

            // Load the PST file within a using block to ensure disposal
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Verify integrity by calculating total items count across all folders
                int totalItemsCount = GetTotalItemsCount(pst.RootFolder);
                Console.WriteLine($"Total items count: {totalItemsCount}");

                // Enumerate root subfolders to further validate structure
                foreach (FolderInfo folderInfo in pst.RootFolder.GetSubFolders())
                {
                    Console.WriteLine($"Folder: {folderInfo.DisplayName}, Items: {folderInfo.ContentCount}, Unread: {folderInfo.ContentUnreadCount}");
                }
            }
        }
        catch (Exception ex)
        {
            // Log any unexpected errors
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Recursively calculates the total number of items in a folder and its subfolders
    private static int GetTotalItemsCount(FolderInfo folder)
    {
        int count = folder.ContentCount;
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            count += GetTotalItemsCount(subFolder);
        }
        return count;
    }
}
