using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailTaskExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Define output directory and ensure it exists
                string outputDirectory = "Output";
                if (!Directory.Exists(outputDirectory))
                {
                    Directory.CreateDirectory(outputDirectory);
                }

                // Define file paths for the initial and completed task files
                string initialTaskPath = Path.Combine(outputDirectory, "task_initial.msg");
                string completedTaskPath = Path.Combine(outputDirectory, "task_completed.msg");

                // Create a MAPI task with 0% completion
                using (MapiTask task = new MapiTask())
                {
                    task.Subject = "Sample Task";
                    task.Body = "This is a sample task created with Aspose.Email.";
                    task.StartDate = DateTime.Now;
                    task.DueDate = DateTime.Now.AddDays(7);
                    task.PercentComplete = 0;

                    // Save the task with zero percent completion
                    try
                    {
                        task.Save(initialTaskPath, TaskSaveFormat.Msg);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error saving initial task: {ex.Message}");
                        return;
                    }

                    // Update the task to 100% completion
                    task.PercentComplete = 100;

                    // Save the updated task
                    try
                    {
                        task.Save(completedTaskPath, TaskSaveFormat.Msg);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error saving completed task: {ex.Message}");
                        return;
                    }
                }

                Console.WriteLine("Task files created successfully.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Unexpected error: {ex.Message}");
            }
        }
    }
}
