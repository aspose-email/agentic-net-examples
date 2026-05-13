using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Define the path for the task message file
            string taskFilePath = "task.msg";

            // Ensure the directory exists
            string taskDirectory = Path.GetDirectoryName(taskFilePath);
            if (!string.IsNullOrEmpty(taskDirectory) && !Directory.Exists(taskDirectory))
            {
                Directory.CreateDirectory(taskDirectory);
            }

            // Guard file write operation
            try
            {
                // Create a new MapiTask and set its properties
                using (MapiTask task = new MapiTask())
                {
                    task.Subject = "Sample Task";
                    task.Body = "This is a sample task with estimated effort.";
                    task.StartDate = DateTime.Now;
                    task.DueDate = DateTime.Now.AddDays(2);
                    // Estimated effort is in minutes (8 hours = 480 minutes)
                    task.EstimatedEffort = 480;

                    // Save the task to a MSG file
                    task.Save(taskFilePath, TaskSaveFormat.Msg);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error writing task file: {ex.Message}");
                return;
            }

            // Guard file read operation
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

                Console.Error.WriteLine("Aspose.Email.Calendar.Task file does not exist.");
                return;
            }

            try
            {
                // Load the MSG file as a MapiMessage
                using (MapiMessage loadedMessage = MapiMessage.Load(taskFilePath))
                {
                    // Verify that the loaded message is a task
                    if (loadedMessage.SupportedType != MapiItemType.Task)
                    {
                        Console.Error.WriteLine("Loaded message is not a task.");
                        return;
                    }

                    // Convert the message to a MapiTask
                    MapiTask loadedTask = (MapiTask)loadedMessage.ToMapiMessageItem();

                    // Verify the EstimatedEffort value
                    int expectedEffort = 480;
                    if (loadedTask.EstimatedEffort == expectedEffort)
                    {
                        Console.WriteLine($"Estimated effort verified: {loadedTask.EstimatedEffort} minutes.");
                    }
                    else
                    {
                        Console.WriteLine($"Estimated effort mismatch. Expected: {expectedEffort}, Actual: {loadedTask.EstimatedEffort}");
                    }

                    // Dispose the loaded task explicitly
                    loadedTask.Dispose();
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error reading task file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
