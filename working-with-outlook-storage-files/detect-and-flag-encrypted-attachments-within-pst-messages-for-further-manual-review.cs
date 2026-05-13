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
            string pstFilePath = "sample.pst";

            // Ensure PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstFilePath))
            {
                try
                {
                    using (PersonalStorage placeholderPst = PersonalStorage.Create(pstFilePath, FileFormatVersion.Unicode))
                    {
                        // Create a default Inbox folder so the PST is usable
                        placeholderPst.CreatePredefinedFolder("Inbox", StandardIpmFolder.Inbox);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST file for reading
            using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
            {
                // Iterate through all subfolders starting from the root
                foreach (FolderInfo folder in pst.RootFolder.GetSubFolders())
                {
                    ProcessFolder(pst, folder);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    private static void ProcessFolder(PersonalStorage pst, FolderInfo folder)
    {
        // Enumerate messages in the current folder
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            try
            {
                // Extract the full MAPI message
                using (MapiMessage message = pst.ExtractMessage(messageInfo))
                {
                    // If the message itself is encrypted, flag its attachments
                    if (message.IsEncrypted)
                    {
                        foreach (MapiAttachment attachment in message.Attachments)
                        {
                            Console.WriteLine($"Encrypted attachment detected: Folder='{folder.DisplayName}', MessageSubject='{message.Subject}', AttachmentFileName='{attachment.FileName}'");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to process message ID '{messageInfo.EntryIdString}': {ex.Message}");
            }
        }

        // Recursively process subfolders
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            ProcessFolder(pst, subFolder);
        }
    }
}
