using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        const string pstPath = "storage.pst";

        // Ensure a placeholder PST file exists
        if (!File.Exists(pstPath))
        {
            using (PersonalStorage pst = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
            {
                // Create an empty root folder (already exists) – no additional setup needed
            }
            Console.WriteLine($"Placeholder PST file created at: {pstPath}");
        }

        try
        {
            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Process the root folder and all its subfolders recursively
                ProcessFolder(pst, pst.RootFolder);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    // Recursively processes a folder, extracts recipient addresses from each message
    static void ProcessFolder(PersonalStorage pst, FolderInfo folder)
    {
        Console.WriteLine($"Folder: {folder.DisplayName}");
        Console.WriteLine($"Total items: {folder.ContentCount}");
        Console.WriteLine($"Unread items: {folder.ContentUnreadCount}");

        // Enumerate messages in the current folder
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            Console.WriteLine($"Subject: {messageInfo.Subject}");

            // Extract the full message to access recipients
            MapiMessage message = pst.ExtractMessage(messageInfo);
            if (message?.Recipients != null)
            {
                foreach (MapiRecipient recipient in message.Recipients)
                {
                    Console.WriteLine($"Recipient: {recipient.EmailAddress}");
                }
            }
        }

        // Recurse into subfolders
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            ProcessFolder(pst, subFolder);
        }
    }
}
