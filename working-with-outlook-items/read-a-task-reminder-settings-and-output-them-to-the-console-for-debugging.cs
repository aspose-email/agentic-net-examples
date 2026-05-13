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
            string taskFilePath = "task.msg";

            // Ensure the task file exists; create a minimal placeholder if it does not.
            if (!File.Exists(taskFilePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(taskFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                try
                {
                    using (MapiTask placeholderTask = new MapiTask())
                    {
                        placeholderTask.Subject = "Sample Task";
                        placeholderTask.DueDate = DateTime.Now.AddDays(1);
                        placeholderTask.ReminderSet = true;
                        placeholderTask.ReminderTime = DateTime.Now.AddHours(1);
                        placeholderTask.ReminderFileParameter = "default.wav";
                        // Flags property is read‑only; no need to set it for this example.
                        placeholderTask.Save(taskFilePath, TaskSaveFormat.Msg);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder task file: {ex.Message}");
                    return;
                }
            }

            // Load the task message and output its reminder settings.
            try
            {
                using (MapiMessage message = MapiMessage.Load(taskFilePath))
                {
                    if (message.SupportedType == MapiItemType.Task)
                    {
                        using (MapiTask task = (MapiTask)message.ToMapiMessageItem())
                        {
                            Console.WriteLine($"Reminder Set: {task.ReminderSet}");
                            Console.WriteLine($"Reminder Time: {task.ReminderTime}");
                            Console.WriteLine($"Reminder File Parameter: {task.ReminderFileParameter}");
                            bool resetReminder = task.Flags.HasFlag(MapiTaskFlags.ResetReminder);
                            Console.WriteLine($"Reset Reminder Flag: {resetReminder}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("The specified file does not contain a task.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing task file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
