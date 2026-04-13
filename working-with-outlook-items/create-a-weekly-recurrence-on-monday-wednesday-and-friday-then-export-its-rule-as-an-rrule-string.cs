using Aspose.Email;
using System;
using Aspose.Email.Calendar;
using Aspose.Email.Calendar.Recurrences;

class Program
{
    static void Main()
    {
        try
        {
            // Define the start date of the recurrence (today)
            DateTime startDate = DateTime.Today;

            // Create a weekly recurrence pattern starting on the specified date
            WeeklyRecurrencePattern weeklyPattern = new WeeklyRecurrencePattern(startDate);

            // Set the days of the week on which the event occurs: Monday, Wednesday, Friday
            weeklyPattern.StartDays = new CalendarDay[]
            {
                CalendarDay.Monday,
                CalendarDay.Wednesday,
                CalendarDay.Friday
            };

            // Set the interval to 1 week (optional, default is 1)
            weeklyPattern.Interval = 1;

            // Export the recurrence rule as an RRULE string
            string rrule = weeklyPattern.ToString();

            Console.WriteLine("RRULE: " + rrule);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
