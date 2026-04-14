using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Output file path
            string outputPath = "DailyRecurrence.msg";

            // Ensure the output directory exists
            string directory = Path.GetDirectoryName(Path.GetFullPath(outputPath));
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            // Create a MAPI calendar item
            using (MapiCalendar calendar = new MapiCalendar())
            {
                calendar.StartDate = DateTime.Today.AddHours(9);
                calendar.EndDate = DateTime.Today.AddHours(10);
                calendar.Location = "Conference Room";
                calendar.Subject = "Daily Standup Meeting";

                // Define a daily recurrence pattern that excludes weekends
                MapiCalendarDailyRecurrencePattern pattern = new MapiCalendarDailyRecurrencePattern();
                pattern.StartDate = calendar.StartDate;
                pattern.EndDate = calendar.StartDate.AddMonths(1);
                pattern.DayOfWeek = MapiCalendarDayOfWeek.Monday |
                                    MapiCalendarDayOfWeek.Tuesday |
                                    MapiCalendarDayOfWeek.Wednesday |
                                    MapiCalendarDayOfWeek.Thursday |
                                    MapiCalendarDayOfWeek.Friday;

                // Attach the recurrence pattern to the calendar
                MapiCalendarEventRecurrence recurrence = new MapiCalendarEventRecurrence();
                recurrence.RecurrencePattern = pattern;
                calendar.Recurrence = recurrence;

                // Save the calendar to a MSG file
                try
                {
                    calendar.Save(outputPath);
                    Console.WriteLine($"Calendar saved to {outputPath}");
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to save calendar: {ex.Message}");
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
