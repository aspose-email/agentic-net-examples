using Aspose.Email.Calendar.Recurrences;
using System;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Create a concrete recurrence pattern and set it to never end.
            MapiCalendarYearlyAndMonthlyRecurrencePattern pattern = new MapiCalendarYearlyAndMonthlyRecurrencePattern();
            pattern.EndType = MapiCalendarRecurrenceEndType.NeverEnd;
            pattern.StartDate = new DateTime(2024, 1, 1);

            // Associate the pattern with a calendar event recurrence.
            MapiCalendarEventRecurrence eventRecurrence = new MapiCalendarEventRecurrence();
            eventRecurrence.RecurrencePattern = pattern;

            // Display the configured EndType.
            Console.WriteLine("Recurrence EndType set to: " + eventRecurrence.RecurrencePattern.EndType);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
