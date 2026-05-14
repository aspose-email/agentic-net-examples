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
            string outputPath = "task_without_attachment.msg";

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

                Console.Error.WriteLine($"Input file '{inputPath}' does not exist.");
                return;
            }

            using (MapiMessage message = MapiMessage.Load(inputPath))
            {
                if (message.SupportedType != MapiItemType.Task)
                {
                    Console.Error.WriteLine("The provided file is not a task.");
                    return;
                }

                using (MapiTask task = (MapiTask)message.ToMapiMessageItem())
                {
                    if (task.Attachments != null && task.Attachments.Count > 0)
                    {
                        task.Attachments.Clear();
                    }

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
