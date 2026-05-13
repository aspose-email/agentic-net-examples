using Aspose.Email;
using System;
using Aspose.Email.Calendar.Recurrences;

namespace Example
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Create a daily recurrence pattern with an interval of three days
                // and set it to end after twenty occurrences.
                DailyRecurrencePattern recurrencePattern = new DailyRecurrencePattern(20, 3);

                // Generate the RRULE string.
                string rrule = recurrencePattern.ToString();

                Console.WriteLine("RRULE: " + rrule);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Error: " + ex.Message);
                return;
            }
        }
    }
}
