using Aspose.Email.Mapi;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mime;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string pstPath = "sample.pst";

            // Ensure a PST file exists – create a minimal one if missing
            if (!File.Exists(pstPath))
            {
                CreateSamplePst(pstPath);
                Console.WriteLine($"Created placeholder PST file at: {pstPath}");
            }

            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                FolderInfo rootFolder = pst.RootFolder;
                PrintFolder(rootFolder, 0);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    private static void CreateSamplePst(string path)
    {
        // Create a new PST file
        using (PersonalStorage pst = PersonalStorage.Create(path, FileFormatVersion.Unicode))
        {
            // Add a subfolder
            FolderInfo inbox = pst.RootFolder.AddSubFolder("Inbox");

            // Create a simple email message
            MailMessage message = new MailMessage
            {
                Subject = "Sample Message",
                Body = "This is a sample message created for demonstration purposes.",
                From = "sender@example.com",
                To = "recipient@example.com"
            };

            // Add the message to the Inbox folder
            inbox.AddMessage(MapiMessage.FromMailMessage(message));
        }
    }

    private static void PrintFolder(FolderInfo folder, int level)
    {
        string indent = new string(' ', level * 2);
        Console.WriteLine($"{indent}Folder: {folder.DisplayName} (Items: {folder.ContentCount}, Unread: {folder.ContentUnreadCount})");

        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            Console.WriteLine($"{indent}  Message: {messageInfo.Subject}");
        }

        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            PrintFolder(subFolder, level + 1);
        }
    }
}
