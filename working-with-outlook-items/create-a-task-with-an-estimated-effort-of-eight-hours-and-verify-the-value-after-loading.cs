using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Define the task file path
            string taskFilePath = "task.msg";

            // Ensure the directory for the task file exists
            string taskDirectory = Path.GetDirectoryName(taskFilePath);
            if (!string.IsNullOrEmpty(taskDirectory) && !Directory.Exists(taskDirectory))
            {
                Directory.CreateDirectory(taskDirectory);
            }

            // Create an ExchangeTask with an estimated effort of eight hours (480 minutes)
            using (ExchangeTask task = new ExchangeTask())
            {
                task.Subject = "Sample Task";
                task.ActualWork = 480; // minutes actually worked
                task.TotalWork = 480;  // minutes expected to work

                // Save the task to a MSG file
                task.Save(taskFilePath, Aspose.Email.Mapi.TaskSaveFormat.Msg);
            }

            // Verify the saved task by loading it as a MapiMessage
            if (File.Exists(taskFilePath))
            {
                using (MapiMessage message = MapiMessage.Load(taskFilePath))
                {
                    if (message.SupportedType == Aspose.Email.Mapi.MapiItemType.Task)
                    {
                        MapiTask loadedTask = (MapiTask)message.ToMapiMessageItem();
                        Console.WriteLine($"Verified Estimated Effort (minutes): {loadedTask.EstimatedEffort}");
                    }
                    else
                    {
                        Console.Error.WriteLine("The saved file is not a task item.");
                    }
                }
            }
            else
            {
                Console.Error.WriteLine("Task file was not created.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
