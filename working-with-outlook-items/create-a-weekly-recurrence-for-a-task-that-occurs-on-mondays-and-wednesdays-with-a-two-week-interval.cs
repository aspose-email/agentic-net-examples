using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Calendar.Recurrences;
using Aspose.Email.Clients.Exchange.WebService;

class Program
{
    static void Main()
    {
        try
        {
            // Output file path for the task
            string outputPath = "WeeklyTask.msg";

            // Ensure the output directory exists
            string directory = Path.GetDirectoryName(Path.GetFullPath(outputPath));
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create a task and configure its basic properties
            using (ExchangeTask task = new ExchangeTask())
            {
                task.Subject = "Bi‑weekly Task";
                task.StartDate = DateTime.Today;
                task.DueDate = DateTime.Today.AddDays(1);
                task.Body = "Aspose.Email.Calendar.Task occurs every Monday and Wednesday with a two‑week interval.";

                // Create a weekly recurrence pattern with a 2‑week interval
                WeeklyRecurrencePattern recurrence = new WeeklyRecurrencePattern(DateTime.Today, 2);
                // Optional: set an end date or occurrence count
                // recurrence.EndDate = DateTime.Today.AddMonths(3);

                task.RecurrencePattern = recurrence;

                // Save the task to a MSG file
                task.Save(outputPath);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
