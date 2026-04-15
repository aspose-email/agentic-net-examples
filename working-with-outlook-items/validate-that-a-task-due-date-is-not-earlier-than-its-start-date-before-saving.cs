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
            // Create a task instance
            using (Aspose.Email.Calendar.Task task = new Aspose.Email.Calendar.Task())
            {
                task.Subject = "Sample Task";
                task.StartDate = new DateTime(2023, 10, 10);
                task.DueDate = new DateTime(2023, 10, 5); // Example of an invalid due date

                // Validate that the due date is not earlier than the start date
                if (task.DueDate < task.StartDate)
                {
                    Console.Error.WriteLine("Due date cannot be earlier than start date.");
                    return;
                }

                string outputPath = "task.msg";

                // Ensure the target directory exists
                string directory = Path.GetDirectoryName(outputPath);
                if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                try
                {
                    // Save the task in MSG format
                    task.Save(outputPath, TaskSaveFormat.Msg);
                    Console.WriteLine($"Aspose.Email.Calendar.Task saved successfully to '{outputPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error saving task: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
