using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email.Storage.Pst;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string pstPath = "sample.pst";

            // Ensure PST file exists; create minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    using (PersonalStorage pstCreate = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // Create a default folder and a dummy message
                        FolderInfo inboxFolder = pstCreate.RootFolder.AddSubFolder("Inbox");
                        MapiMessage dummyMessage = new MapiMessage("sender@example.com", "recipient@example.com", "Placeholder Subject", "Placeholder body");
                        inboxFolder.AddMessage(dummyMessage);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Open the PST file for reading
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    // Keywords to search for in message subjects
                    List<string> keywords = new List<string> { "Invoice", "Report", "Meeting" };

                    // Start recursive search from the root folder
                    SearchFolder(pst.RootFolder, keywords);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing PST file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Recursively searches a folder and its subfolders for messages whose subjects contain any of the keywords
    static void SearchFolder(FolderInfo folder, List<string> keywords)
    {
        // Enumerate messages in the current folder
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            if (messageInfo.Subject != null)
            {
                foreach (string keyword in keywords)
                {
                    if (messageInfo.Subject.IndexOf(keyword, StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        Console.WriteLine($"Found matching subject: {messageInfo.Subject}");
                        break;
                    }
                }
            }
        }

        // Recurse into subfolders
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            SearchFolder(subFolder, keywords);
        }
    }
}
