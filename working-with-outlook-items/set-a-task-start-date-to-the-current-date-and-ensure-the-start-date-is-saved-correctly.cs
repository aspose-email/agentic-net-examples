using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Output file path for the task
            string outputPath = "Task.msg";

            // Ensure the directory exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create a new task and set its start date to the current date
            using (Aspose.Email.Calendar.Task task = new Aspose.Email.Calendar.Task())
            {
                task.StartDate = DateTime.Now;
                task.Subject = "Sample Task";

                // Save the task in MSG format
                task.Save(outputPath, Aspose.Email.Mapi.TaskSaveFormat.Msg);
            }

            Console.WriteLine("Aspose.Email.Calendar.Task saved successfully to " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
