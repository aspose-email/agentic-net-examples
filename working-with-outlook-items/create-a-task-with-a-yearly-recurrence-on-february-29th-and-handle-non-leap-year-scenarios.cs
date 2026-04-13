using Aspose.Email;
using System;
using System.Collections.Generic;
using System.IO;
using Aspose.Email.Calendar.Recurrences;

class Program
{
    static void Main()
    {
        try
        {
            // Define the output file for the generated occurrence dates
            string outputPath = "Feb29Occurrences.txt";

            // Ensure the directory for the output file exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            // Create a yearly recurrence pattern that occurs on February 29th.
            // The iCalendar rule "FREQ=YEARLY;BYMONTH=2;BYMONTHDAY=29" expresses this.
            CalendarRecurrence recurrence = new CalendarRecurrence("FREQ=YEARLY;BYMONTH=2;BYMONTHDAY=29");

            // Set the start date to a known leap‑year date (Feb 29, 2020)
            // Set an end date several years later to demonstrate handling of non‑leap years
            recurrence.EndDate = new DateTime(2028, 2, 28); // end just before a leap day to stop after 2028

            // Generate all occurrence dates between StartDate and EndDate.
            // CalendarRecurrence handles non‑leap years by simply skipping those years.
            List<DateTime> occurrences = recurrence.GenerateOccurrences();

            // Write the occurrences to the console and to the output file.
            using (StreamWriter writer = new StreamWriter(outputPath, false))
            {
                foreach (DateTime occ in occurrences)
                {
                    string line = occ.ToString("yyyy-MM-dd");
                    Console.WriteLine(line);
                    writer.WriteLine(line);
                }
            }

            Console.WriteLine($"Occurrences have been written to '{outputPath}'.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
