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
            string inputPath = "task.ics";
            string originalPath = "originalTask.msg";
            string clonePath = "cloneTask.msg";

            // Ensure the input .ics file exists; create a minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (StreamWriter writer = new StreamWriter(inputPath))
                    {
                        writer.WriteLine("BEGIN:VCALENDAR");
                        writer.WriteLine("VERSION:2.0");
                        writer.WriteLine("BEGIN:VTODO");
                        writer.WriteLine("SUMMARY:Placeholder Task");
                        writer.WriteLine("DTSTART:20240101T090000Z");
                        writer.WriteLine("DUE:20240102T090000Z");
                        writer.WriteLine("END:VTODO");
                        writer.WriteLine("END:VCALENDAR");
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder task file: {ex.Message}");
                    return;
                }
            }

            // Load the task from the .ics file
            MapiTask originalTask = null;
            try
            {
                originalTask = MapiTask.FromVTodo(inputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load task: {ex.Message}");
                return;
            }

            using (originalTask)
            {
                // Save the original task as MSG
                try
                {
                    originalTask.Save(originalPath, TaskSaveFormat.Msg);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save original task: {ex.Message}");
                    return;
                }

                // Clone the task by copying its properties
                MapiTask clonedTask = new MapiTask();
                try
                {
                    clonedTask.Subject = originalTask.Subject;
                    clonedTask.Body = originalTask.Body;
                    clonedTask.StartDate = originalTask.StartDate;
                    clonedTask.DueDate = originalTask.DueDate;
                    clonedTask.Priority = originalTask.Priority;
                    clonedTask.PercentComplete = originalTask.PercentComplete;

                    // Modify the clone's subject
                    clonedTask.Subject = clonedTask.Subject + " - Clone";

                    // Save the cloned task as MSG
                    clonedTask.Save(clonePath, TaskSaveFormat.Msg);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to clone or save task: {ex.Message}");
                }
                finally
                {
                    clonedTask.Dispose();
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
