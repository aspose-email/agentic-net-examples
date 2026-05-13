using Aspose.Email;
using System;
using Aspose.Email.Calendar.Recurrences;

class Program
{
    static void Main()
    {
        try
        {
            // iCalendar definition with DTSTART and a complex RRULE containing BYDAY and BYMONTHDAY
            string iCalendarString = "DTSTART:20230101T090000\nRRULE:FREQ=MONTHLY;BYDAY=MO,WE,FR;BYMONTHDAY=1,15;COUNT=10";

            // Parse the recurrence pattern from the iCalendar string
            CalendarRecurrence recurrence = CalendarRecurrence.FromiCalendar(iCalendarString);

            // Generate all occurrences defined by the recurrence rule
            DateCollection occurrences = recurrence.GenerateOccurrences();

            Console.WriteLine("Generated occurrences:");
            foreach (DateTime occurrence in occurrences)
            {
                Console.WriteLine(occurrence.ToString("yyyy-MM-dd HH:mm"));
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
