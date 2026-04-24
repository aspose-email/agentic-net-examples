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

            // Ensure the PST file exists; create a minimal placeholder if it does not.
            if (!File.Exists(pstPath))
            {
                try
                {
                    using (PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Empty PST created.
                    }
                }
                catch (Exception createEx)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {createEx.Message}");
                    return;
                }
            }

            // Open the PST file.
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Process the root folder and all subfolders.
                ProcessFolder(pst, pst.RootFolder);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    private static void ProcessFolder(PersonalStorage pst, FolderInfo folder)
    {
        int totalAttachments = 0;

        // Enumerate all messages in the current folder.
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            try
            {
                // Extract attachments for the current message.
                MapiAttachmentCollection attachments = pst.ExtractAttachments(messageInfo);
                if (attachments != null)
                {
                    totalAttachments += attachments.Count;
                }
            }
            catch (Exception msgEx)
            {
                Console.Error.WriteLine($"Failed to extract attachments from message '{messageInfo.Subject}': {msgEx.Message}");
            }
        }

        Console.WriteLine($"Folder '{folder.DisplayName}' - Total Attachments: {totalAttachments}");

        // Recursively process subfolders.
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            ProcessFolder(pst, subFolder);
        }
    }
}
