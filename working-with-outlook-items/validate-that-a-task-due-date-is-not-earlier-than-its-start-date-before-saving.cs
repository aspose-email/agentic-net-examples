using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            // Define output file path
            string outputPath = "Task.msg";

            // Ensure the output directory exists
            string outputDir = Path.GetDirectoryName(Path.GetFullPath(outputPath));
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Create a new ExchangeTask
            using (ExchangeTask task = new ExchangeTask())
            {
                task.Subject = "Project Plan";
                task.Body = "Complete the project milestones.";
                task.StartDate = new DateTime(2024, 12, 1, 9, 0, 0);
                task.DueDate = new DateTime(2024, 12, 15, 17, 0, 0);

                // Validate that DueDate is not earlier than StartDate
                if (task.DueDate < task.StartDate)
                {
                    Console.Error.WriteLine("Error: DueDate cannot be earlier than StartDate.");
                    return;
                }

                // Save the task to a MSG file
                try
                {
                    task.Save(outputPath);
                    Console.WriteLine($"Aspose.Email.Calendar.Task saved successfully to '{outputPath}'.");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save task: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
