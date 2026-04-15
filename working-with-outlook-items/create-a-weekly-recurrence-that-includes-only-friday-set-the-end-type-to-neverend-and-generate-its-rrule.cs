using System;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Create a weekly recurrence pattern
            MapiCalendarWeeklyRecurrencePattern weeklyPattern = new MapiCalendarWeeklyRecurrencePattern();

            // Include only Friday
            weeklyPattern.DayOfWeek = MapiCalendarDayOfWeek.Friday;

            // Set the recurrence to never end
            weeklyPattern.EndType = MapiCalendarRecurrenceEndType.NeverEnd;

            // Define start date and interval (every week)
            weeklyPattern.StartDate = DateTime.Now;
            weeklyPattern.Period = 1; // 1 week interval

            // Generate the RRULE string
            string rrule = weeklyPattern.ToString();
            Console.WriteLine("RRULE: " + rrule);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
