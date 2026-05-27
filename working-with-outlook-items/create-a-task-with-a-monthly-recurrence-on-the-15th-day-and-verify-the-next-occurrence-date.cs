using Aspose.Email;
using System;
using System.Collections.Generic;
using Aspose.Email.Calendar.Recurrences;

class Program
{
    static void Main()
    {
        try
        {
            // Define a monthly recurrence on the 15th day using an iCalendar RRULE string
            string rrule = "RRULE:FREQ=MONTHLY;BYMONTHDAY=15";
            CalendarRecurrence recurrence = CalendarRecurrence.FromiCalendar(rrule);

            // Set the start date for the recurrence (today)
            // Generate the next occurrence (first occurrence after the start date)
            List<DateTime> occurrences = recurrence.GenerateOccurrences(1);
            if (occurrences.Count > 0)
            {
                DateTime nextOccurrence = occurrences[0];
                Console.WriteLine($"Next occurrence date: {nextOccurrence:yyyy-MM-dd}");
            }
            else
            {
                Console.WriteLine("No occurrences were generated.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
