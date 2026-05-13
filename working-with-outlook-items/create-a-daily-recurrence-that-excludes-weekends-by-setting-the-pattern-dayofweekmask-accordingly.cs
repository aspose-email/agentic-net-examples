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
            // Create a weekly recurrence pattern that repeats every week
            // but only on weekdays (Monday through Friday).
            MapiCalendarWeeklyRecurrencePattern recurrence = new MapiCalendarWeeklyRecurrencePattern();

            // Set the day-of-week mask to include Monday, Tuesday, Wednesday,
            // Thursday, and Friday. The enum is flagged, so we combine values.
            recurrence.DayOfWeek = MapiCalendarDayOfWeek.Monday |
                                   MapiCalendarDayOfWeek.Tuesday |
                                   MapiCalendarDayOfWeek.Wednesday |
                                   MapiCalendarDayOfWeek.Thursday |
                                   MapiCalendarDayOfWeek.Friday;

            // Define the start and end dates for the recurrence.
            recurrence.EndDate = new DateTime(2023, 1, 31);

            // Set the period to 1 week (repeat every week).
            recurrence.Period = 1;

            Console.WriteLine("Recurrence pattern created:");
            Console.WriteLine("DayOfWeek mask: " + recurrence.DayOfWeek);
            Console.WriteLine("StartDate: " + recurrence.StartDate);
            Console.WriteLine("EndDate: " + recurrence.EndDate);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
