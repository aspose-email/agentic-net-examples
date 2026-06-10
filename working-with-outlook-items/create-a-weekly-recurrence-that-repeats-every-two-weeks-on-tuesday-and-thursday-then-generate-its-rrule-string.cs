using Aspose.Email;
using System;
using Aspose.Email.Calendar.Recurrences;

class Program
{
    static void Main()
    {
        try
        {
            // Define the start date for the recurrence (today)
            DateTime startDate = DateTime.Today;

            // Create a weekly recurrence pattern that repeats every 2 weeks
            WeeklyRecurrencePattern recurrence = new WeeklyRecurrencePattern(startDate, 2);

            // Set the days of the week on which the event occurs: Tuesday and Thursday

            // Optionally limit the number of occurrences (e.g., 10 occurrences)
            recurrence.Occurs = 10;

            // Generate the RRULE string representation of the recurrence pattern
            string rrule = recurrence.ToString();

            Console.WriteLine("RRULE: " + rrule);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
