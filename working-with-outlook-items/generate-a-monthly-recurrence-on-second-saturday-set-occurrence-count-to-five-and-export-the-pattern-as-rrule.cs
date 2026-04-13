using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar.Recurrences;

class Program
{
    static void Main()
    {
        try
        {
            // Define the output file path for the RRULE string
            string outputPath = "recurrence.rrule";

            // Ensure the directory for the output file exists
            string directory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create a monthly recurrence pattern for the second Saturday of each month
            MonthlyRecurrencePattern pattern = new MonthlyRecurrencePattern(DayPosition.Second, CalendarDay.Saturday, 1);
            pattern.Occurs = 5; // Set the number of occurrences to five

            // Export the recurrence pattern as an RRULE string
            string rrule = pattern.ToString();

            // Write the RRULE string to the output file
            try
            {
                File.WriteAllText(outputPath, rrule);
                Console.WriteLine("RRULE saved to " + outputPath);
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine("Failed to write RRULE: " + ioEx.Message);
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine("Error: " + ex.Message);
        }
    }
}
