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
            // Define the start date for the recurrence
            DateTime startDate = DateTime.Today;

            // Create a daily recurrence pattern that occurs 15 times
            DailyRecurrencePattern dailyPattern = new DailyRecurrencePattern(startDate);
            dailyPattern.Occurs = 15; // stop after fifteen occurrences

            // Generate the RRULE string
            string rrule = dailyPattern.ToString();
            Console.WriteLine("Generated RRULE: " + rrule);

            // Validate the RRULE syntax by parsing it back into a CalendarRecurrence object
            CalendarRecurrence recurrence = CalendarRecurrence.FromiCalendar(rrule);
            if (recurrence != null)
            {
                Console.WriteLine("RRULE syntax is valid.");
            }
            else
            {
                Console.WriteLine("RRULE syntax validation failed.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
