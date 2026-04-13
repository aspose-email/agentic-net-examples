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
            // Define a recurrence that could generate many occurrences.
            CalendarRecurrence recurrence = new CalendarRecurrence();
            recurrence.EndDate = DateTime.Today.AddYears(10); // far future end date

            // Outlook limits the number of occurrences (commonly 999).
            int outlookMaxOccurrences = 999;

            // Generate a large number of occurrences to test the limit.
            List<DateTime> occurrences = recurrence.GenerateOccurrences(2000);

            if (occurrences.Count > outlookMaxOccurrences)
            {
                Console.WriteLine($"Recurrence exceeds Outlook maximum of {outlookMaxOccurrences} occurrences. Generated: {occurrences.Count}");
            }
            else
            {
                Console.WriteLine($"Recurrence is within Outlook limit. Generated: {occurrences.Count}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
        }
    }
}
