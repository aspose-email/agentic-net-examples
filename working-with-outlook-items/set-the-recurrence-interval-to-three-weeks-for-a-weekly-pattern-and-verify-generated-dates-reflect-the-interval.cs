using Aspose.Email.Calendar.Recurrences;
using Aspose.Email;
using System;
using System.Collections.Generic;
using Aspose.Email.Mapi;

namespace RecurrenceExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Define the start date of the recurrence
                DateTime startDate = new DateTime(2023, 1, 1);

                // Create a weekly recurrence pattern with a 3‑week interval
                MapiCalendarWeeklyRecurrencePattern weeklyPattern = new MapiCalendarWeeklyRecurrencePattern();
                weeklyPattern.StartDate = startDate;
                weeklyPattern.Period = 3; // Interval in weeks
                weeklyPattern.EndDate = startDate.AddMonths(2); // Limit the range for demonstration

                // Generate occurrence dates based on the interval
                List<DateTime> occurrenceDates = new List<DateTime>();
                DateTime currentDate = weeklyPattern.StartDate;

                while (currentDate <= weeklyPattern.EndDate)
                {
                    occurrenceDates.Add(currentDate);
                    // Advance by the specified number of weeks
                    currentDate = currentDate.AddDays(7 * weeklyPattern.Period);
                }

                // Output the generated dates to verify the 3‑week interval
                Console.WriteLine("Weekly recurrence with a 3‑week interval:");
                foreach (DateTime date in occurrenceDates)
                {
                    Console.WriteLine(date.ToString("yyyy-MM-dd"));
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                return;
            }
        }
    }
}
