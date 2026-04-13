using System;
using Aspose.Email;
using Aspose.Email.Calendar.Recurrences;

class Program
{
    static void Main()
    {
        try
        {
            // Create a monthly recurrence that occurs on the 15th day of each month
            CalendarRecurrence recurrence = new CalendarRecurrence("FREQ=MONTHLY;BYMONTHDAY=15");
            // Generate the next occurrence date
            var nextOccurrences = recurrence.GenerateOccurrences(1);
            if (nextOccurrences.Count > 0)
            {
                Console.WriteLine("Next occurrence date: " + nextOccurrences[0].ToString("yyyy-MM-dd"));
            }
            else
            {
                Console.WriteLine("No occurrences generated.");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
