using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            string outputPath = "output/task.msg";
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            using (MapiTask task = new MapiTask())
            {
                task.Subject = "Project Plan";
                task.DueDate = DateTime.Now.AddDays(14);
                // Set the history status of the task
                task.History = MapiTaskHistory.Assigned;
                // Save the task to MSG format
                task.Save(outputPath, TaskSaveFormat.Msg);
                Console.WriteLine($"Aspose.Email.Calendar.Task saved with history {task.History} to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
