using Aspose.Email;
using System;
using Aspose.Email.Calendar.Recurrences;

class Program
{
    static void Main()
    {
        try
        {
            // Outlook's maximum allowed occurrences for a recurrence pattern
            const int OutlookMaxOccurrences = 9999;

            // Desired recurrence settings
            int desiredOccurrences = 12000; // Example value exceeding the limit
            int intervalDays = 1; // Every day

            // Create a daily recurrence pattern using the constructor that accepts occurrences and interval
            DailyRecurrencePattern dailyPattern = new DailyRecurrencePattern(desiredOccurrences, intervalDays);

            // Validate against Outlook's maximum
            if (dailyPattern.Occurs > OutlookMaxOccurrences)
            {
                Console.WriteLine($"Recurrence exceeds Outlook's limit of {OutlookMaxOccurrences} occurrences. Truncating to the maximum allowed.");
                dailyPattern.Occurs = OutlookMaxOccurrences;
            }
            else
            {
                Console.WriteLine("Recurrence is within Outlook's allowed limits.");
            }

            // Display the final recurrence settings
            Console.WriteLine($"Occurrences: {dailyPattern.Occurs}");
            Console.WriteLine($"Interval (days): {dailyPattern.Interval}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
