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

            // Ensure the PST file exists; create a minimal placeholder if missing.
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create a new Unicode PST file.
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Created placeholder PST at '{pstPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
                // No messages to process in a newly created PST.
                return;
            }

            // Open the PST file.
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Get the Inbox folder.
                FolderInfo inbox = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);

                // Ensure target folders exist (High, Normal, Low).
                EnsureSubFolder(pst.RootFolder, "High", out FolderInfo highFolder);
                EnsureSubFolder(pst.RootFolder, "Normal", out FolderInfo normalFolder);
                EnsureSubFolder(pst.RootFolder, "Low", out FolderInfo lowFolder);

                // Iterate through messages in the Inbox and move them based on importance.
                foreach (MessageInfo msgInfo in inbox.EnumerateMessages())
                {
                    switch (msgInfo.Importance)
                    {
                        case MapiImportance.High:
                            pst.MoveItem(msgInfo, highFolder);
                            break;
                        case MapiImportance.Low:
                            pst.MoveItem(msgInfo, lowFolder);
                            break;
                        default:
                            pst.MoveItem(msgInfo, normalFolder);
                            break;
                    }
                }

                Console.WriteLine("Messages have been sorted into High, Normal, and Low folders.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Helper method to create a subfolder if it does not already exist.
    private static void EnsureSubFolder(FolderInfo parent, string folderName, out FolderInfo folder)
    {
        try
        {
            folder = parent.GetSubFolder(folderName);
        }
        catch
        {
            // Subfolder does not exist; create it.
            parent.AddSubFolder(folderName);
            folder = parent.GetSubFolder(folderName);
        }
    }
}
