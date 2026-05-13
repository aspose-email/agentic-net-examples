using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            string directoryPath = "MsgFiles";

            // Ensure the directory exists; create if missing and add a placeholder MSG file.
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
                string placeholderPath = Path.Combine(directoryPath, "placeholder.msg");

                // Create a minimal task and save it as MSG.
                MapiTask placeholderTask = new MapiTask(
                    "Placeholder Task",
                    "This is a placeholder task.",
                    DateTime.Now,
                    DateTime.Now.AddDays(1));

                placeholderTask.Save(placeholderPath, TaskSaveFormat.Msg);
            }

            // Get all .msg files in the directory.
            string[] msgFiles = Directory.GetFiles(directoryPath, "*.msg");

            foreach (string msgFile in msgFiles)
            {
                try
                {
                    // Load the MSG file.
                    using (MapiMessage message = MapiMessage.Load(msgFile))
                    {
                        // Check if the message is a task.
                        if (message.SupportedType == MapiItemType.Task)
                        {
                            // Convert to MapiTask to access task properties.
                            MapiTask task = (MapiTask)message.ToMapiMessageItem();
                            Console.WriteLine($"File: {Path.GetFileName(msgFile)} - Task Subject: {task.Subject}");
                        }
                        else
                        {
                            Console.WriteLine($"File: {Path.GetFileName(msgFile)} - Not a task item.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing file '{msgFile}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
