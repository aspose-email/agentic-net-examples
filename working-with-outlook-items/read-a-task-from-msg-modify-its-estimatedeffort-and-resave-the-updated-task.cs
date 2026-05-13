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
            string outputPath = "updated_task.msg";

            // Verify input file exists
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

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            try
            {
                using (MapiMessage msg = MapiMessage.Load(inputPath))
                {
                    if (msg.SupportedType == MapiItemType.Task)
                    {
                        // Convert to MapiTask
                        MapiTask task = (MapiTask)msg.ToMapiMessageItem();

                        // Modify EstimatedEffort (minutes)
                        task.EstimatedEffort = 120;

                        // Save the updated task to a new MSG file
                        task.Save(outputPath, TaskSaveFormat.Msg);
                        Console.WriteLine($"Task updated and saved to: {outputPath}");
                    }
                    else
                    {
                        Console.Error.WriteLine("The provided MSG file does not contain a task.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing MSG file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
