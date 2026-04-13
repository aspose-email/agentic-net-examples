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
            string outputPath = "task.msg";

            // Ensure the directory exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create a new MapiTask
            using (MapiTask task = new MapiTask())
            {
                task.Subject = "Weekly Task";
                task.StartDate = DateTime.Today;
                task.DueDate = DateTime.Today.AddDays(1);

                // Configure weekly recurrence: Mondays and Wednesdays, every 2 weeks
                MapiCalendarWeeklyRecurrencePattern weeklyPattern = new MapiCalendarWeeklyRecurrencePattern();
                weeklyPattern.DayOfWeek = MapiCalendarDayOfWeek.Monday | MapiCalendarDayOfWeek.Wednesday;
                weeklyPattern.Period = 2; // two‑week interval
                weeklyPattern.StartDate = DateTime.Today;

                // Assign the recurrence pattern to the task
                task.Recurrence = weeklyPattern;

                // Save the task to a MSG file
                task.Save(outputPath, TaskSaveFormat.Msg);
            }

            Console.WriteLine("Task with weekly recurrence saved to " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
