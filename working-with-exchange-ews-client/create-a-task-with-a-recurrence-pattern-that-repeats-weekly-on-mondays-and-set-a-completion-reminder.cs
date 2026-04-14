using System;
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
            // Create a weekly recurrence pattern that occurs every week on Monday
            WeeklyRecurrencePattern recurrence = new WeeklyRecurrencePattern(DateTime.Now, 1);
            recurrence.EndDate = DateTime.Now.AddMonths(3); // optional end date

            // Create an Exchange task and assign the recurrence pattern
            ExchangeTask task = new ExchangeTask();
            task.Subject = "Weekly Review Task";
            task.StartDate = DateTime.Now;
            task.DueDate = DateTime.Now.AddDays(1);
            task.ReminderDate = DateTime.Now.AddHours(2); // completion reminder
            task.RecurrencePattern = recurrence;

            // Output task details to verify creation
            Console.WriteLine("Task Subject: " + task.Subject);
            Console.WriteLine("Start Date: " + task.StartDate);
            Console.WriteLine("Due Date: " + task.DueDate);
            Console.WriteLine("Reminder Date: " + task.ReminderDate);
            Console.WriteLine("Recurs Weekly on: " + ((WeeklyRecurrencePattern)task.RecurrencePattern).StartDays);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
