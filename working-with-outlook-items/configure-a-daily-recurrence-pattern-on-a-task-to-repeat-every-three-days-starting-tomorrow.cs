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
            string outputPath = Path.Combine(Directory.GetCurrentDirectory(), "task.msg");
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create a new MAPI task
            using (MapiTask task = new MapiTask())
            {
                task.Subject = "Sample Recurring Task";
                task.Body = "This task repeats every three days starting tomorrow.";
                task.StartDate = DateTime.Today.AddDays(1);
                task.DueDate = task.StartDate.AddDays(1);

                // Configure daily recurrence pattern
                MapiCalendarDailyRecurrencePattern dailyPattern = new MapiCalendarDailyRecurrencePattern
                {
                    Period = 3, // repeat every 3 days
                    StartDate = task.StartDate,
                    EndType = MapiCalendarRecurrenceEndType.NeverEnd
                };

                task.Recurrence = dailyPattern;

                // Save the task to a MSG file
                try
                {
                    task.Save(outputPath, TaskSaveFormat.Msg);
                    Console.WriteLine($"Task saved to: {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save task: {ex.Message}");
                    return;
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
