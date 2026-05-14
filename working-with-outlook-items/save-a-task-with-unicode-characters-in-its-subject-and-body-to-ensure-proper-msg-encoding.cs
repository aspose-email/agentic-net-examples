using Aspose.Email;
using System;
using System.IO;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Output file path for the task
            string outputPath = "UnicodeTask.msg";

            // Ensure the directory for the output file exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create a task with Unicode characters in subject and body
            using (Aspose.Email.Calendar.Task task = new Aspose.Email.Calendar.Task())
            {
                task.Subject = "任务 – Пример – مثال 🚀";
                task.Body = "This is a task body with Unicode characters: 中文, العربية, हिन्दी, 😊";

                // Save the task as MSG (Unicode encoding)
                task.Save(outputPath, TaskSaveFormat.Msg);
            }

            Console.WriteLine("Aspose.Email.Calendar.Task saved successfully to " + outputPath);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
