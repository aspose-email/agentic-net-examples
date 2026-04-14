using System;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Calendar.Recurrences;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define a complex RRULE string with BYDAY and BYMONTHDAY components
            string rrule = "RRULE:FREQ=MONTHLY;BYDAY=MO,TU;BYMONTHDAY=15,30";

            // Parse the RRULE into a CalendarRecurrence object
            CalendarRecurrence recurrence = CalendarRecurrence.FromiCalendar(rrule);

            // Set the start and end dates for the recurrence generation
            recurrence.EndDate = new DateTime(2023, 12, 31);

            // Generate all occurrence dates within the specified range
            DateCollection occurrences = recurrence.GenerateOccurrences();

            // Output the generated dates
            foreach (DateTime occurrence in occurrences)
            {
                Console.WriteLine(occurrence);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
