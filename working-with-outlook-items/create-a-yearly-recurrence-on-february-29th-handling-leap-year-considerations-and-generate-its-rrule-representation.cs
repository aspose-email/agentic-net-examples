using Aspose.Email;
using System;
using Aspose.Email.Calendar.Recurrences;

class Program
{
    static void Main()
    {
        try
        {
            // Create a yearly recurrence that occurs on February 29th.
            // The constructor takes the zero‑based day offset (28 for the 29th day) and the month.
            YearlyRecurrencePattern yearlyPattern = new YearlyRecurrencePattern(28, CalendarMonth.February);
            yearlyPattern.Interval = 1; // Every year

            // Generate the iCalendar RRULE string.
            string rrule = yearlyPattern.ToString();

            Console.WriteLine("RRULE: " + rrule);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
