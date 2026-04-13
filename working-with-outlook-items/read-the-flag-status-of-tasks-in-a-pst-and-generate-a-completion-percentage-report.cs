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
            string pstPath = "tasks.pst";

            // Ensure the PST file exists; create a minimal placeholder if missing.
            if (!File.Exists(pstPath))
            {
                try
                {
                    using (PersonalStorage placeholder = PersonalStorage.Create(pstPath, FileFormatVersion.Unicode))
                    {
                        // No tasks are added; placeholder PST is empty.
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder PST: {ex.Message}");
                    return;
                }
            }

            // Open the PST file.
            try
            {
                using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
                {
                    int totalTasks = 0;
                    int accumulatedPercent = 0;

                    // Helper to process messages in a folder.
                    void ProcessFolder(FolderInfo folder)
                    {
                        foreach (MessageInfo messageInfo in folder.EnumerateMessages())
                        {
                            using (MapiMessage mapiMessage = pst.ExtractMessage(messageInfo))
                            {
                                if (mapiMessage.SupportedType == MapiItemType.Task)
                                {
                                    // Convert the MAPI message to a MapiTask.
                                    MapiTask task = (MapiTask)mapiMessage.ToMapiMessageItem();
                                    totalTasks++;
                                    accumulatedPercent += task.PercentComplete;
                                }
                            }
                        }

                        // Recursively process subfolders.
                        foreach (FolderInfo subFolder in folder.GetSubFolders())
                        {
                            ProcessFolder(subFolder);
                        }
                    }

                    // Start processing from the root folder.
                    ProcessFolder(pst.RootFolder);

                    // Generate the completion percentage report.
                    if (totalTasks == 0)
                    {
                        Console.WriteLine("No tasks found in the PST.");
                    }
                    else
                    {
                        double averageCompletion = (double)accumulatedPercent / totalTasks;
                        Console.WriteLine($"Total tasks: {totalTasks}");
                        Console.WriteLine($"Average completion: {averageCompletion:F2}%");
                    }
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
}
