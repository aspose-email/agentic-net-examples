using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string inputPath = "task.msg";
            string outputPath = "task_updated.msg";

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

            using (MapiMessage msg = MapiMessage.Load(inputPath))
            {
                if (msg.SupportedType != MapiItemType.Task)
                {
                    Console.Error.WriteLine("The MSG file does not contain a task.");
                    return;
                }

                MapiTask task = (MapiTask)msg.ToMapiMessageItem();
                task.PercentComplete = 75;
                task.Save(outputPath, TaskSaveFormat.Msg);
                Console.WriteLine($"Task updated and saved to '{outputPath}'.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
