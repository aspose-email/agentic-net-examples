using System;
using Aspose.Email;
using Aspose.Email.Clients.Exchange.WebService;
using Aspose.Email.Calendar.Recurrences;

namespace AsposeEmailRecurringTaskDemo
{
    // Author: Aspose.Email example author
    class Program
    {
        static void Main()
        {
            try
            {
                // Define task details
                DateTime startDate = DateTime.Today.AddHours(9); // today at 9 AM
                DateTime dueDate = startDate.AddHours(1); // due in 1 hour
                DateTime recurrenceEndDate = DateTime.Today.AddDays(5); // repeat for 5 days

                // Create a new Exchange task
                ExchangeTask task = new ExchangeTask
                {
                    Subject = "Daily recurring task",
                    StartDate = startDate,
                    DueDate = dueDate,
                    // Set daily recurrence with an end date
                    RecurrencePattern = new DailyRecurrencePattern(recurrenceEndDate)
                };

                // Output task information
                Console.WriteLine("Aspose.Email.Calendar.Task created:");
                Console.WriteLine($"Subject: {task.Subject}");
                Console.WriteLine($"Start: {task.StartDate}");
                Console.WriteLine($"Due: {task.DueDate}");
                Console.WriteLine($"Recurs daily until: {task.RecurrencePattern.EndDate:d}");

                // Verify that recurrence stops after the end date
                DateTime nextOccurrence = task.StartDate;
                int occurrenceCount = 0;
                while (nextOccurrence <= task.RecurrencePattern.EndDate)
                {
                    occurrenceCount++;
                    nextOccurrence = nextOccurrence.AddDays(1);
                }

                Console.WriteLine($"Total occurrences generated: {occurrenceCount}");
                Console.WriteLine("No further occurrences are generated after the end date.");
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return;
            }
        }
    }
}
