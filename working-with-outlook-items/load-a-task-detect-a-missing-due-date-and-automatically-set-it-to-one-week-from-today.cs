using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "task.msg";
            string outputPath = "updated_task.msg";

            // Ensure the input file exists; if not, create a minimal placeholder task.
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (Task placeholderTask = new Task())
                    {
                        placeholderTask.Subject = "Placeholder Task";
                        placeholderTask.StartDate = DateTime.Now;
                        placeholderTask.DueDate = DateTime.Now.AddDays(1);
                        placeholderTask.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder task: {ex.Message}");
                    return;
                }
            }

            // Load the task from file.
            Task loadedTask;
            try
            {
                loadedTask = Task.Load(inputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load task: {ex.Message}");
                return;
            }

            using (loadedTask)
            {
                // Detect missing due date (DateTime.MinValue indicates not set).
                if (loadedTask.DueDate == DateTime.MinValue)
                {
                    loadedTask.DueDate = DateTime.Now.AddDays(7);
                    Console.WriteLine("Due date was missing; set to one week from today.");
                }
                else
                {
                    Console.WriteLine($"Existing due date: {loadedTask.DueDate}");
                }

                // Save the updated task.
                try
                {
                    loadedTask.Save(outputPath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save updated task: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
