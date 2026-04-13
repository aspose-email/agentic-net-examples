using Aspose.Email;
using System;
using Aspose.Email.Calendar.Recurrences;

class Program
{
    static void Main()
    {
        try
        {
            // Create a daily recurrence pattern that occurs 20 times with an interval of 3 days
            DailyRecurrencePattern recurrencePattern = new DailyRecurrencePattern(20, 3);

            // Generate the RRULE string representation
            string rrule = recurrencePattern.ToString();

            Console.WriteLine("Generated RRULE:");
            Console.WriteLine(rrule);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
