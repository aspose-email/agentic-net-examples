using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Define output file path
            string outputDirectory = "Output";
            string outputPath = Path.Combine(outputDirectory, "Task.msg");

            // Ensure the output directory exists
            if (!Directory.Exists(outputDirectory))
            {
                try
                {
                    Directory.CreateDirectory(outputDirectory);
                }
                catch (Exception dirEx)
                {
                    Console.Error.WriteLine($"Failed to create directory '{outputDirectory}': {dirEx.Message}");
                    return;
                }
            }

            // Create a new Aspose.Email.Calendar.Task and set its start date to the current date and time
            using (Aspose.Email.Calendar.Task task = new Aspose.Email.Calendar.Task())
            {
                task.Subject = "Sample Task";
                task.StartDate = DateTime.Now;
                task.DueDate = DateTime.Now.AddDays(7);
                task.Body = "This task was created with the start date set to the current date.";

                // Save the task to a MSG file
                try
                {
                    task.Save(outputPath);
                    Console.WriteLine($"Aspose.Email.Calendar.Task saved successfully to '{outputPath}'.");
                }
                catch (Exception saveEx)
                {
                    Console.Error.WriteLine($"Failed to save task: {saveEx.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
