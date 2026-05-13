using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Calendar.Recurrences;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define an iCalendar RRULE string with COUNT and INTERVAL parameters.
            string rrule = "FREQ=DAILY;COUNT=5;INTERVAL=2";

            // Create a CalendarRecurrence instance from the RRULE string.
            CalendarRecurrence recurrence = new CalendarRecurrence(rrule);

            // Set the start date of the recurrence (e.g., Jan 1, 2023 at 09:00).
            // Generate the occurrence dates based on the recurrence pattern.
            IList<DateTime> occurrences = recurrence.GenerateOccurrences();

            // Output the generated occurrence dates to the console.
            Console.WriteLine("Generated occurrence dates:");
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
