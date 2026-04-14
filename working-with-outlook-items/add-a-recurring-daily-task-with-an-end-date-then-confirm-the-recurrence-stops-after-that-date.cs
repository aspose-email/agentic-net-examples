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
            // Create a daily recurring task with an end date
            using (ExchangeTask task = new ExchangeTask())
            {
                task.Subject = "Daily Report";
                task.StartDate = DateTime.Today;
                task.DueDate = DateTime.Today.AddHours(1);
                task.Body = "Complete the daily report.";

                // Daily recurrence starting today, every 1 day, ending after 5 days
                DailyRecurrencePattern recurrence = new DailyRecurrencePattern(DateTime.Today, 1);
                recurrence.EndDate = DateTime.Today.AddDays(5);
                task.RecurrencePattern = recurrence;

                // Confirm the recurrence end date
                if (task.RecurrencePattern is DailyRecurrencePattern dailyPattern)
                {
                    Console.WriteLine($"Aspose.Email.Calendar.Task will recur daily until {dailyPattern.EndDate:yyyy-MM-dd}");
                }

                // Save the task to a MSG file (file I/O guarded)
                string outputPath = "DailyTask.msg";
                string outputDir = Path.GetDirectoryName(outputPath);
                if (string.IsNullOrEmpty(outputDir))
                {
                    outputDir = Environment.CurrentDirectory;
                }

                try
                {
                    if (!Directory.Exists(outputDir))
                    {
                        Directory.CreateDirectory(outputDir);
                    }

                    task.Save(outputPath);
                    Console.WriteLine($"Aspose.Email.Calendar.Task saved to {outputPath}");
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
