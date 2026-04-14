using System;
using System.IO;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Output file path for the task
            string outputPath = "task.msg";

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create a new MapiTask instance
            using (MapiTask task = new MapiTask())
            {
                // Set basic task properties
                task.Subject = "Sample Task";
                task.Body = "This is a sample task created with Aspose.Email.";

                // Prepare a list of status updates (history entries)
                List<MapiTaskHistory> historyUpdates = new List<MapiTaskHistory>
                {
                    MapiTaskHistory.Assigned,
                    MapiTaskHistory.DueDateChanged,
                    MapiTaskHistory.Accepted
                };

                // Assign the last status update to the History property
                if (historyUpdates.Count > 0)
                {
                    task.History = historyUpdates[historyUpdates.Count - 1];
                }

                // Save the task to a MSG file
                try
                {
                    task.Save(outputPath, TaskSaveFormat.Msg);
                    Console.WriteLine($"Task saved to {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error saving task: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
