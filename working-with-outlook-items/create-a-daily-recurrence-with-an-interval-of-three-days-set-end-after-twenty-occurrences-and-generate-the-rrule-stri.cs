using Aspose.Email;
using System;
using Aspose.Email.Calendar.Recurrences;

class Program
{
    static void Main()
    {
        try
        {
            // Create a daily recurrence pattern with an interval of three days
            // and set it to end after twenty occurrences.
            DailyRecurrencePattern dailyPattern = new DailyRecurrencePattern(occurs: 20, interval: 3);

            // Generate the RRULE string representation.
            string rrule = dailyPattern.ToString();

            Console.WriteLine("RRULE: " + rrule);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
