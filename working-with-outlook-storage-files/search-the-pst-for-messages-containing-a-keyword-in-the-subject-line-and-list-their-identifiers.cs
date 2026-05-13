using Aspose.Email.Mapi;
using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mime;
using Aspose.Email.Storage.Pst;

class Program
{
    static void Main()
    {
        try
        {
            string pstFilePath = "sample.pst";

            // Create a minimal PST file if it does not exist (placeholder for testing)
            if (!File.Exists(pstFilePath))
            {
                // Create a new PST file
                using (PersonalStorage pst = PersonalStorage.Create(pstFilePath, FileFormatVersion.Unicode))
                {
                    // Add a sample message with a subject containing the keyword
                    MailMessage msg = new MailMessage
                    {
                        Subject = "Important: Project Update",
                        Body = "This is a test message."
                    };
                    pst.RootFolder.AddMessage(MapiMessage.FromMailMessage(msg));
                }
            }

            string keyword = "Important";

            using (PersonalStorage pst = PersonalStorage.FromFile(pstFilePath))
            {
                FolderInfo rootFolder = pst.RootFolder;
                SearchFolder(rootFolder, keyword);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    static void SearchFolder(FolderInfo folder, string keyword)
    {
        // Search messages in the current folder
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            if (!string.IsNullOrEmpty(messageInfo.Subject) &&
                messageInfo.Subject.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                Console.WriteLine($"Message ID: {messageInfo.EntryIdString}, Subject: {messageInfo.Subject}");
            }
        }

        // Recursively search subfolders
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            SearchFolder(subFolder, keyword);
        }
    }
}
