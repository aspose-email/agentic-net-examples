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

            // Ensure the task file exists; create a minimal placeholder if missing
            if (!File.Exists(taskFilePath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage())
                    {
                        placeholder.Save(taskFilePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder task file: {ex.Message}");
                    return;
                }
            }

            // Load the message and process the task
            try
            {
                using (MapiMessage msg = MapiMessage.Load(taskFilePath))
                {
                    if (msg.SupportedType != MapiItemType.Task)
                    {
                        Console.WriteLine("The loaded message is not a task.");
                        return;
                    }

                    // Convert to MapiTask
                    MapiTask task = (MapiTask)msg.ToMapiMessageItem();

                    DateTime reminderTime = task.ReminderTime;
                    DateTime systemTime = DateTime.Now;

                    if (reminderTime != systemTime)
                    {
                        Console.WriteLine($"Discrepancy detected:");
                        Console.WriteLine($"  Stored Reminder Time: {reminderTime:O}");
                        Console.WriteLine($"  System Current Time : {systemTime:O}");
                    }
                    else
                    {
                        Console.WriteLine("Reminder time matches the system time.");
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
