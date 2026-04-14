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
            // The constructor takes the day offset (29) and the month (February).
            YearlyRecurrencePattern yearlyRecurrence = new YearlyRecurrencePattern(29, CalendarMonth.February);
            
            // Optionally define a start and end date for the recurrence.
            // Start on the next leap year date.
            // End after a few occurrences (e.g., after 3 years).
            yearlyRecurrence.EndDate = new DateTime(2032, 2, 29);
            
            // Generate the RRULE string representation.
            string rrule = yearlyRecurrence.ToString();
            Console.WriteLine("RRULE: " + rrule);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
