using System;
using Aspose.Email;
using Aspose.Email.Calendar.Recurrences;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Initialize a weekly recurrence pattern starting today with a 1‑week interval
            DateTime startDate = DateTime.Today;
            WeeklyRecurrencePattern recurrencePattern = new WeeklyRecurrencePattern(startDate, 1);

            // Set the days of week on which the event occurs: Tuesday, Wednesday, Friday

            // Export the recurrence rule as an iCalendar RRULE string
            string rrule = recurrencePattern.ToString();

            Console.WriteLine("RRULE: " + rrule);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
