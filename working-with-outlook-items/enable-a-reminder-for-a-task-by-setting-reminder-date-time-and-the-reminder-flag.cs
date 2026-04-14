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
            // Define output file path
            string outputFilePath = "task_with_reminder.msg";

            // Ensure the output directory exists
            string outputDirectory = Path.GetDirectoryName(outputFilePath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create a new MAPI task and configure reminder properties
            using (MapiTask task = new MapiTask())
            {
                task.Subject = "Sample Task with Reminder";
                task.Body = "This task has a reminder set.";
                task.DueDate = DateTime.Now.AddDays(2);
                task.StartDate = DateTime.Now;
                task.ReminderSet = true;
                task.ReminderTime = DateTime.Now.AddHours(1); // Reminder after 1 hour

                // Save the task to a MSG file
                try
                {
                    task.Save(outputFilePath, TaskSaveFormat.Msg);
                    Console.WriteLine($"Task saved successfully to '{outputFilePath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error saving task: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
