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
            string pstPath = "sample.pst";

            // Ensure the PST file exists; create a minimal placeholder if it does not.
            if (!File.Exists(pstPath))
            {
                using (PersonalStorage pstCreate = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                {
                    // Create default folders (Inbox and Sent Items) for demonstration.
                    pstCreate.CreatePredefinedFolder("Inbox", StandardIpmFolder.Inbox);
                    pstCreate.CreatePredefinedFolder("Sent Items", StandardIpmFolder.SentItems);
                }
                Console.WriteLine($"Created placeholder PST at {pstPath}");
            }

            // Open the existing PST file.
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Retrieve source and destination folders.
                FolderInfo sourceFolder = pst.GetPredefinedFolder(StandardIpmFolder.Inbox);
                FolderInfo destinationFolder = pst.GetPredefinedFolder(StandardIpmFolder.SentItems);

                // Move each message from the source folder to the destination folder.
                foreach (MessageInfo messageInfo in sourceFolder.EnumerateMessages())
                {
                    pst.MoveItem(messageInfo, destinationFolder);
                    Console.WriteLine($"Moved message: {messageInfo.Subject}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
