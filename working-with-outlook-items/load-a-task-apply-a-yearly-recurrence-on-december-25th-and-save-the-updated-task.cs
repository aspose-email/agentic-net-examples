using Aspose.Email.Calendar.Recurrences;
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
            string inputPath = "task.msg";
            string outputPath = "updatedTask.msg";

            // Ensure the input file exists; create a minimal placeholder if missing.
            if (!File.Exists(inputPath))
            {
                using (MapiTask placeholder = new MapiTask("Sample Task", "Placeholder body", DateTime.Now, DateTime.Now.AddDays(1)))
                {
                    placeholder.Save(inputPath, TaskSaveFormat.Msg);
                }
            }

            // Load the existing task message.
            using (MapiMessage message = MapiMessage.Load(inputPath))
            {
                if (message.SupportedType != MapiItemType.Task)
                {
                    Console.Error.WriteLine("The provided file is not a task.");
                    return;
                }

                // Convert the message to a MapiTask.
                MapiTask task = (MapiTask)message.ToMapiMessageItem();

                // Create a yearly recurrence pattern for December 25th.
                var yearlyRecurrence = new MapiCalendarYearlyAndMonthlyRecurrencePattern
                {
                    Day = 25,
                    StartDate = new DateTime(DateTime.Now.Year, 12, 25),
                    EndDate = new DateTime(DateTime.Now.Year + 5, 12, 25),
                    PatternType = MapiCalendarRecurrencePatternType.Month // Using month pattern to represent yearly on a specific day.
                };

                // Assign the recurrence to the task.
                task.Recurrence = yearlyRecurrence;

                // Save the updated task.
                task.Save(outputPath, TaskSaveFormat.Msg);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
