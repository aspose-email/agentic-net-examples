using Aspose.Email;
using System;
using Aspose.Email.Calendar.Recurrences;

class Program
{
    static void Main()
    {
        try
        {
            // Create a monthly recurrence pattern for the second Saturday of each month
            MonthlyRecurrencePattern pattern = new MonthlyRecurrencePattern(DayPosition.Second, CalendarDay.Saturday, 1);
            // Set the number of occurrences to five
            pattern.Occurs = 5;
            // Export the recurrence pattern as an RRULE string
            string rrule = pattern.ToString();
            Console.WriteLine("RRULE: " + rrule);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
