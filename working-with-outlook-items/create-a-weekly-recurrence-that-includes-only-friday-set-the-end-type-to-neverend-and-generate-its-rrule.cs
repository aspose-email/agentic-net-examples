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
            // Create a weekly recurrence pattern starting today, repeating every week
            WeeklyRecurrencePattern weeklyPattern = new WeeklyRecurrencePattern(DateTime.Today, 1);

            // Include only Friday in the recurrence

            // No end date is set, which corresponds to an infinite recurrence (NeverEnd)

            // Generate the RRULE string
            string rrule = weeklyPattern.ToString();

            Console.WriteLine("Generated RRULE:");
            Console.WriteLine(rrule);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
