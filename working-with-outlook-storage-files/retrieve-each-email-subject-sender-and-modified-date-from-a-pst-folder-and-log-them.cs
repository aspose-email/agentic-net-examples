using Aspose.Email.Mapi;
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
                try
                {
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Placeholder PST created at '{pstPath}'.");
                }
                catch (Exception createEx)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {createEx.Message}");
                    return;
                }
            }

            // Open the PST file and process its folders.
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    ProcessFolder(pst.RootFolder, pst);
                }
            }
            catch (Exception pstEx)
            {
                Console.Error.WriteLine($"Error accessing PST file: {pstEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Recursively processes a folder and its subfolders.
    private static void ProcessFolder(FolderInfo folder, PersonalStorage pst)
    {
        // Enumerate messages in the current folder.
        try
        {
            foreach (MessageInfo messageInfo in folder.EnumerateMessages())
            {
                try
                {
                    using (MapiMessage message = pst.ExtractMessage(messageInfo))
                    {
                        string subject = messageInfo.Subject;
                        string sender = message.SenderName ?? message.SenderEmailAddress;
                        DateTime modified = message.ClientSubmitTime;

                        Console.WriteLine($"Folder: {folder.DisplayName}");
                        Console.WriteLine($"Subject: {subject}");
                        Console.WriteLine($"Sender: {sender}");
                        Console.WriteLine($"Modified: {modified}");
                        Console.WriteLine();
                    }
                }
                catch (Exception msgEx)
                {
                    Console.Error.WriteLine($"Failed to extract message: {msgEx.Message}");
                }
            }
        }
        catch (Exception enumEx)
        {
            Console.Error.WriteLine($"Failed to enumerate messages in folder '{folder.DisplayName}': {enumEx.Message}");
        }

        // Recursively process subfolders.
        try
        {
            foreach (FolderInfo subFolder in folder.GetSubFolders())
            {
                ProcessFolder(subFolder, pst);
            }
        }
        catch (Exception subEx)
        {
            Console.Error.WriteLine($"Failed to enumerate subfolders of '{folder.DisplayName}': {subEx.Message}");
        }
    }
}
