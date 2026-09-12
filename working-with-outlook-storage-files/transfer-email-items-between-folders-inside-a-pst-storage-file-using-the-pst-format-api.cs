using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

namespace AsposeEmailPstTransfer
{
    class Program
    {
        static void Main()
        {
            const string pstPath = "sample.pst";

            // Ensure the PST file exists; create a minimal placeholder if missing.
            try
            {
                if (!File.Exists(pstPath))
                {
                    // Create a new PST file with Unicode format.
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Created placeholder PST at '{pstPath}'.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                return;
            }

            // Open the PST and transfer messages between folders.
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Get source (Inbox) and destination (Deleted Items) predefined folders.
                    FolderInfo sourceFolder = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);
                    FolderInfo destinationFolder = pst.GetPredefinedFolder(StandardIpmFolder.DeletedItems);

                    // Enumerate all messages in the source folder.
                    foreach (MessageInfo messageInfo in sourceFolder.EnumerateMessages())
                    {
                        // Move each message to the destination folder.
                        pst.MoveItem(messageInfo, destinationFolder);
                    }

                    Console.WriteLine($"Moved messages from '{sourceFolder.DisplayName}' to '{destinationFolder.DisplayName}'.");
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing PST file: {ex.Message}");
                return;
            }
        }
    }
}
