using Aspose.Email;
using System;
using Aspose.Email.Calendar.Recurrences;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Define the start date of the recurrence (a Monday)
            DateTime startDate = new DateTime(2023, 1, 2);

            // Create a weekly recurrence pattern with an interval of 1 week
            WeeklyRecurrencePattern recurrence = new WeeklyRecurrencePattern(startDate, 1);

            // Set the days of week on which the event occurs: Monday, Wednesday, Friday

            // Export the recurrence rule as an RRULE string
            string rrule = recurrence.ToString();

            Console.WriteLine("RRULE: " + rrule);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
