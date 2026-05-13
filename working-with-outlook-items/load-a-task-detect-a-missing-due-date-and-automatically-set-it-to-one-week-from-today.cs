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
            string taskFilePath = "task.msg";

            // Ensure the task file exists before attempting to load it
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

                Console.Error.WriteLine($"File '{taskFilePath}' does not exist.");
                return;
            }

            // Load the MSG file as a MapiMessage
            using (MapiMessage msg = MapiMessage.Load(taskFilePath))
            {
                // Verify that the message actually represents a task
                if (msg.SupportedType != MapiItemType.Task)
                {
                    Console.Error.WriteLine("The specified file does not contain a task.");
                    return;
                }

                // Convert the message to a MapiTask object
                MapiTask task = (MapiTask)msg.ToMapiMessageItem();

                // Detect missing due date (default DateTime) and set it to one week from today
                if (task.DueDate == DateTime.MinValue)
                {
                    task.DueDate = DateTime.Today.AddDays(7);
                    Console.WriteLine("Due date was missing. Set to one week from today.");
                }
                else
                {
                    Console.WriteLine($"Existing due date: {task.DueDate}");
                }

                // Save the updated task back to the same file in MSG format
                task.Save(taskFilePath, TaskSaveFormat.Msg);
                Console.WriteLine("Aspose.Email.Calendar.Task saved successfully.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
