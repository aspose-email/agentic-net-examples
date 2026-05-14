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
            // Define the folder containing Outlook MSG files
            string inputFolder = "Tasks";

            // Verify the folder exists
            if (!Directory.Exists(inputFolder))
            {
                Console.Error.WriteLine($"Input folder '{inputFolder}' does not exist.");
                return;
            }

            // Get all .msg files in the folder
            string[] msgFiles;
            try
            {
                msgFiles = Directory.GetFiles(inputFolder, "*.msg");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to enumerate files: {ex.Message}");
                return;
            }

            foreach (string filePath in msgFiles)
            {
                // Ensure the file exists before processing
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
                    // Load the MSG file as a MapiMessage
                    using (MapiMessage msg = MapiMessage.Load(filePath))
                    {
                        // Process only task items
                        if (msg.SupportedType == MapiItemType.Task)
                        {
                            // Convert to MapiTask
                            MapiTask task = (MapiTask)msg.ToMapiMessageItem();

                            // Validate due date
                            DateTime now = DateTime.Now;
                            if (task.DueDate < now)
                            {
                                Console.WriteLine($"Past due task detected:");
                                Console.WriteLine($"  File: {Path.GetFileName(filePath)}");
                                Console.WriteLine($"  Subject: {task.Subject}");
                                Console.WriteLine($"  DueDate: {task.DueDate}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error processing file '{filePath}': {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
