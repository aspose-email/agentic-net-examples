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

            // Ensure the directory exists
            string taskDirectory = Path.GetDirectoryName(taskFilePath);
            if (!string.IsNullOrEmpty(taskDirectory) && !Directory.Exists(taskDirectory))
            {
                Directory.CreateDirectory(taskDirectory);
            }

            // Guard file existence; create a minimal placeholder if missing
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
                        placeholderTask.DueDate = DateTime.Now.AddDays(7);
                        placeholderTask.Save(taskFilePath, TaskSaveFormat.Msg);
                    }
                    Console.WriteLine("Placeholder task file created at: " + taskFilePath);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine("Failed to create placeholder task file: " + ex.Message);
                }
                return;
            }

            // Load the MSG file
            try
            {
                using (MapiMessage message = MapiMessage.Load(taskFilePath))
                {
                    if (message.SupportedType == MapiItemType.Task)
                    {
                        MapiTask task = (MapiTask)message.ToMapiMessageItem();
                        using (task)
                        {
                            task.PercentComplete = 75;
                            task.Save(taskFilePath, TaskSaveFormat.Msg);
                            Console.WriteLine("Task updated with PercentComplete = 75% and saved.");
                        }
                    }
                    else
                    {
                        Console.Error.WriteLine("The MSG file does not contain a task.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error processing the MSG file: " + ex.Message);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Unexpected error: " + ex.Message);
        }
    }
}
