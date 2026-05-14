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
            // Define the folder containing MSG files
            string folderPath = "Tasks";

            // Verify the folder exists
            if (!Directory.Exists(folderPath))
            {
                Console.Error.WriteLine($"Folder not found: {folderPath}");
                return;
            }

            // Get all MSG files in the folder
            string[] msgFiles;
            try
            {
                msgFiles = Directory.GetFiles(folderPath, "*.msg");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to enumerate files: {ex.Message}");
                return;
            }

            // Process each MSG file
            foreach (string filePath in msgFiles)
            {
                // Verify the file exists
                if (!File.Exists(filePath))
                {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage(
                        "from@example.com",
                        "to@example.com",
                        "Placeholder Subject",
                        "Placeholder body."))
                    {
                        placeholder.Save(filePath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                    Console.Error.WriteLine($"File not found: {filePath}");
                    continue;
                }

                try
                {
                    // Load the MSG file
                    using (MapiMessage msg = MapiMessage.Load(filePath))
                    {
                        // Check if the message is a task
                        if (msg.SupportedType == MapiItemType.Task)
                        {
                            // Convert to MapiTask
                            MapiTask task = (MapiTask)msg.ToMapiMessageItem();

                            // Filter tasks whose subject contains "Project"
                            if (task.Subject != null && task.Subject.Contains("Project"))
                            {
                                Console.WriteLine($"Task found: {task.Subject}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing file {filePath}: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
