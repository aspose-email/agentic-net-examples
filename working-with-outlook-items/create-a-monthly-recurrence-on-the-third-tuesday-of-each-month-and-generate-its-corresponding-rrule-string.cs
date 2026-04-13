using System;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Calendar.Recurrences;

class Program
{
    static void Main()
    {
        try
        {
            // Create a monthly recurrence pattern for the third Tuesday of each month
            MonthlyRecurrencePattern recurrence = new MonthlyRecurrencePattern(DayPosition.Third, CalendarDay.Tuesday, 1);
            // Generate the RRULE string representation
            string rrule = recurrence.ToString();
            Console.WriteLine("RRULE: " + rrule);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
