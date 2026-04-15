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

            // Ensure the input file exists; create a minimal placeholder if missing
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                try
                {
                    MapiTask placeholderTask = new MapiTask();
                    placeholderTask.Subject = "Placeholder Task";
                    placeholderTask.DueDate = DateTime.Now.AddDays(7);
                    placeholderTask.Save(inputPath, TaskSaveFormat.Msg);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder task file: {ex.Message}");
                    return;
                }
            }

            // Load the existing task message
            using (MapiMessage message = MapiMessage.Load(inputPath))
            {
                if (message.SupportedType != MapiItemType.Task)
                {
                    Console.Error.WriteLine("The specified file does not contain a task.");
                    return;
                }

                MapiTask task = (MapiTask)message.ToMapiMessageItem();

                // Create a yearly recurrence on December 25th
                MapiCalendarYearlyAndMonthlyRecurrencePattern recurrence = new MapiCalendarYearlyAndMonthlyRecurrencePattern();
                recurrence.Day = 25;
                recurrence.EndDate = new DateTime(DateTime.Now.Year + 5, 12, 25);

                task.Recurrence = recurrence;

                // Save the updated task
                string outputPath = "updatedTask.msg";
                task.Save(outputPath, TaskSaveFormat.Msg);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
