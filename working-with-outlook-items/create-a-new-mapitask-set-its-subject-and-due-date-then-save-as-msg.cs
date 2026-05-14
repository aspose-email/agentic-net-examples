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
            // Define output file path
            string outputPath = "task.msg";

            // Ensure the output directory exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create a new MapiTask and set its properties
            using (MapiTask task = new MapiTask())
            {
                task.Subject = "Sample Task";
                task.DueDate = DateTime.Now.AddDays(7);

                // Save the task as MSG
                task.Save(outputPath, TaskSaveFormat.Msg);
            }

            Console.WriteLine("Task saved to " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
