using System;
using System.IO;
using System.Collections.Generic;
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

            // Ensure PST file exists; create a minimal placeholder if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                    Console.WriteLine($"Created placeholder PST at '{pstPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                int totalTasks = 0;
                int sumPercent = 0;

                // Process root folder and all subfolders recursively
                ProcessFolder(pst.RootFolder, ref totalTasks, ref sumPercent, pst);

                // Generate report
                if (totalTasks > 0)
                {
                    double overallCompletion = (double)sumPercent / totalTasks;
                    Console.WriteLine($"Total tasks found: {totalTasks}");
                    Console.WriteLine($"Average completion percentage: {overallCompletion:F2}%");
                }
                else
                {
                    Console.WriteLine("No tasks found in the PST.");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }

    // Recursively processes a folder, updating task counters
    private static void ProcessFolder(FolderInfo folder, ref int totalTasks, ref int sumPercent, PersonalStorage pst)
    {
        // Enumerate messages in the current folder
        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
        {
            try
            {
                using (MapiMessage msg = pst.ExtractMessage(messageInfo))
                {
                    if (msg.SupportedType == MapiItemType.Task)
                    {
                        // Convert to MapiTask via ToMapiMessageItem()
                        var taskItem = (MapiTask)msg.ToMapiMessageItem();
                        int percent = taskItem.PercentComplete;
                        totalTasks++;
                        sumPercent += percent;
                        Console.WriteLine($"Task: {taskItem.Subject}, Completion: {percent}%");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to process message ID {messageInfo.EntryId}: {ex.Message}");
            }
        }

        // Recurse into subfolders
        foreach (FolderInfo subFolder in folder.GetSubFolders())
        {
            ProcessFolder(subFolder, ref totalTasks, ref sumPercent, pst);
        }
    }
}
