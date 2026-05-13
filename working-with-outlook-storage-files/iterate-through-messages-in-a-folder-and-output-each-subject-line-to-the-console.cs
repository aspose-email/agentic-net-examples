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
            string pstPath = "storage.pst";

            // Create a placeholder PST file if it does not exist
            if (!File.Exists(pstPath))
            {
                using (PersonalStorage pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                {
                    // Create a subfolder and add a sample message
                    FolderInfo inbox = pst.RootFolder.AddSubFolder("Inbox");
                    var sampleMessage = new MailMessage(
                        "sender@example.com",
                        "receiver@example.com",
                        "Sample Subject",
                        "This is a sample email body.");

                    // Convert MailMessage to MapiMessage before adding
                    MapiMessage mapiMsg = MapiMessage.FromMailMessage(sampleMessage);
                    inbox.AddMessage(mapiMsg);
                }

                Console.WriteLine($"Placeholder PST file created at: {pstPath}");
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Start enumeration from the root folder
                EnumerateFolderMessages(pst.RootFolder);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    static void EnumerateFolderMessages(FolderInfo folder)
    {
        // Output subjects of messages in the current folder
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            Console.WriteLine($"Subject: {messageInfo.Subject}");
        }

        // Recursively process subfolders
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            EnumerateFolderMessages(subFolder);
        }
    }
}
