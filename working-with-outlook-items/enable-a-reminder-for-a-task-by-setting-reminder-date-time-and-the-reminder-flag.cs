using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Output file for the task
            string outputPath = "task.msg";

            // Ensure the output directory exists
            string directory = Path.GetDirectoryName(Path.GetFullPath(outputPath));
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create and configure the ExchangeTask
            using (ExchangeTask task = new ExchangeTask())
            {
                task.Subject = "Sample Task with Reminder";
                task.Body = "This task has a reminder set.";
                task.StartDate = DateTime.Now;
                task.DueDate = DateTime.Now.AddDays(2);

                // Enable reminder by setting the reminder date and time
                task.ReminderDate = DateTime.Now.AddHours(1);

                // Save the task to a MSG file
                task.Save(outputPath);
                Console.WriteLine($"Task saved to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
