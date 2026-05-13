using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            const string pstPath = "sample.pst";

            // Ensure the PST file exists; create a minimal placeholder if missing.
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create an empty Unicode PST file.
                    using (PersonalStorage placeholder = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // No additional content needed.
                    }
                }
                catch (Exception createEx)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST file: {createEx.Message}");
                    return;
                }
            }

            // Open the PST in read‑only mode.
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath, false))
                {
                    // Total number of messages in the entire PST.
                    int totalMessages = pst.Store.GetTotalItemsCount();
                    Console.WriteLine($"Total messages in PST: {totalMessages}");

                    // Optionally, enumerate each folder and display its message count without loading bodies.
                    FolderInfo rootFolder = pst.RootFolder;
                    int rootCount = GetMessageCount(rootFolder);
                    Console.WriteLine($"Messages counted via folder traversal: {rootCount}");
                }
            }
            catch (Exception openEx)
            {
                Console.Error.WriteLine($"Failed to open PST file: {openEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    private static int GetMessageCount(FolderInfo folder)
    {
        int count = folder.ContentCount;

        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            count += GetMessageCount(subFolder);
        }

        return count;
    }
}
