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

            // Ensure the PST file exists; create a minimal one if missing
            if (!File.Exists(pstPath))
            {
                try
                {
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST file
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath))
            {
                // Get the predefined Tasks folder
                FolderInfo tasksFolder;
                try
                {
                    tasksFolder = pst.GetPredefinedFolder(StandardIpmFolder.Tasks);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to retrieve Tasks folder: {ex.Message}");
                    return;
                }

                // Add fifty tasks with incremental due dates
                for (int i = 1; i <= 50; i++)
                {
                    MapiTask task = new MapiTask
                    {
                        Subject = $"Task {i}",
                        DueDate = DateTime.Today.AddDays(i)
                    };

                    try
                    {
                        tasksFolder.AddMapiMessageItem(task);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to add task {i}: {ex.Message}");
                        // Continue adding remaining tasks
                    }
                }

                Console.WriteLine("Successfully added 50 tasks to the PST.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
