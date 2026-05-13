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
            const string taskFilePath = "task.msg";

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
                        placeholderTask.Subject = "Placeholder Task";
                        // 8 hours * 60 minutes = 480 minutes
                        placeholderTask.EstimatedEffort = 480;
                        placeholderTask.Save(taskFilePath, TaskSaveFormat.Msg);
                    }

                    Console.WriteLine($"Placeholder task file created at '{taskFilePath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder task file: {ex.Message}");
                    return;
                }
            }

            // Load the task message
            try
            {
                using (MapiMessage message = MapiMessage.Load(taskFilePath))
                {
                    if (message.SupportedType != MapiItemType.Task)
                    {
                        Console.Error.WriteLine("The loaded message is not a task.");
                        return;
                    }

                    // Convert the MAPI message to a MapiTask object
                    MapiTask task = (MapiTask)message.ToMapiMessageItem();

                    int estimatedEffortMinutes = task.EstimatedEffort;
                    double estimatedHours = estimatedEffortMinutes / 60.0;
                    double estimatedDays = estimatedHours / 8.0;

                    Console.WriteLine($"Estimated effort: {estimatedEffortMinutes} minutes ({estimatedHours:F2} hours, {estimatedDays:F2} days).");
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
