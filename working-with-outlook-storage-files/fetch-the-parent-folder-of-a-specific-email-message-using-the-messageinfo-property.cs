using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Storage.Pst;
using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";

            // Create a minimal PST file with a sample message if it does not exist
            if (!File.Exists(pstPath))
            {
                PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                using (PersonalStorage pstCreate = PersonalStorage.FromFile(pstPath))
                {
                    FolderInfo inbox = pstCreate.RootFolder.AddSubFolder("Inbox");
                    MailMessage sampleMsg = new MailMessage(
                        "sender@example.com",
                        "receiver@example.com",
                        "Target Message",
                        "This is a sample message body.");
                    inbox.AddMessage(MapiMessage.FromMailMessage(sampleMsg));
                }
                Console.WriteLine("Created placeholder PST file with a sample message.");
            }

            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                string targetSubject = "Target Message";
                FolderInfo parentFolder = FindParentFolder(pst.RootFolder, targetSubject);
                if (parentFolder != null)
                {
                    Console.WriteLine($"Parent folder of message \"{targetSubject}\": {parentFolder.DisplayName}");
                }
                else
                {
                    Console.WriteLine($"Message with subject \"{targetSubject}\" not found.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }

    private static FolderInfo FindParentFolder(FolderInfo folder, string subject)
    {
        // Check messages in the current folder
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            if (!string.IsNullOrEmpty(messageInfo.Subject) &&
                messageInfo.Subject.Equals(subject, StringComparison.OrdinalIgnoreCase))
            {
                return folder; // Current folder is the parent of the found message
            }
        }

        // Recursively search subfolders
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            FolderInfo result = FindParentFolder(subFolder, subject);
            if (result != null)
                return result;
        }

        return null; // Not found in this branch
    }
}
