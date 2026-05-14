using Aspose.Email;
using System;
using Aspose.Email.Calendar.Recurrences;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Create a daily recurrence pattern that occurs 15 times with a 1‑day interval
            DailyRecurrencePattern pattern = new DailyRecurrencePattern(15, 1);

            // Generate the RRULE string representation
            string rrule = pattern.ToString();
            Console.WriteLine("Generated RRULE: " + rrule);

            // Simple validation of the RRULE syntax
            bool isValid = rrule.Contains("FREQ=DAILY") && rrule.Contains("COUNT=15");
            Console.WriteLine("RRULE validation: " + (isValid ? "Valid" : "Invalid"));
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
            return;
        }
    }
}
