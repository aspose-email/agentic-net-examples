using System;
using System.Collections.Generic;
using Aspose.Email;
using Aspose.Email.Calendar.Recurrences;

namespace AsposeEmailRecurrenceExample
{
    class Program
    {
        static void Main()
        {
            try
            {
                // Define the start date of the recurrence
                DateTime startDate = new DateTime(2023, 1, 1, 9, 0, 0);

                // Create a weekly recurrence pattern with an interval of 3 weeks
                WeeklyRecurrencePattern weeklyPattern = new WeeklyRecurrencePattern(startDate, 3);
                // Optionally set an end date (e.g., after 5 occurrences)
                weeklyPattern.EndDate = startDate.AddDays(7 * 3 * 5);

                // Generate the next 5 occurrence dates based on the interval
                List<DateTime> occurrenceDates = new List<DateTime>();
                DateTime currentDate = startDate;
                for (int i = 0; i < 5; i++)
                {
                    occurrenceDates.Add(currentDate);
                    // Add three weeks (21 days) for each next occurrence
                    currentDate = currentDate.AddDays(7 * weeklyPattern.Interval);
                }

                // Output the generated dates to verify the three‑week interval
                Console.WriteLine("Generated occurrence dates (3‑week interval):");
                foreach (DateTime occurrence in occurrenceDates)
                {
                    Console.WriteLine(occurrence.ToString("yyyy-MM-dd HH:mm"));
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
