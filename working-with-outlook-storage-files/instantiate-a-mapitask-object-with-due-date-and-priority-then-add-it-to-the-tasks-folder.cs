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

            // Ensure the PST file exists
            if (!File.Exists(pstPath))
            {
                try
                {
                    // Create a new PST file with Unicode format
                    PersonalStorage.Create(pstPath, FileFormatVersion.Unicode);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create PST file: {ex.Message}");
                    return;
                }
            }

            // Open the PST file (false = read/write)
            using (PersonalStorage pst = PersonalStorage.FromFile(pstPath, false))
            {
                // Get the predefined Tasks folder
                FolderInfo tasksFolder = pst.GetPredefinedFolder(StandardIpmFolder.Tasks);

                // Create a MapiTask with due date and priority
                DateTime startDate = DateTime.Now;
                DateTime dueDate = startDate.AddDays(7);
                MapiTask task = new MapiTask("Sample Task", "Complete the sample code.", startDate, dueDate)
                {
                    Priority = MapiTaskPriority.High // High priority
                };

                // Add the task to the Tasks folder
                try
                {
                    string entryId = tasksFolder.AddMapiMessageItem(task);
                    Console.WriteLine($"Task added with EntryId: {entryId}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to add task: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
