using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

namespace AsposeEmailTaskExample
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                string inputPath = "task.msg";
                string outputPath = "updatedTask.msg";

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

                    Console.Error.WriteLine($"Input file '{inputPath}' not found.");
                    return;
                }

                using (MapiMessage message = MapiMessage.Load(inputPath))
                {
                    if (message.SupportedType != MapiItemType.Task)
                    {
                        Console.Error.WriteLine("The MSG file does not contain a task.");
                        return;
                    }

                    using (MapiTask task = (MapiTask)message.ToMapiMessageItem())
                    {
                        // Modify the EstimatedEffort (minutes)
                        task.EstimatedEffort = 120;

                        // Save the updated task back to MSG format
                        task.Save(outputPath, TaskSaveFormat.Msg);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
