using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Calendar.Recurrences;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "sample.msg";
            string outputPath = "updated.msg";

            // Ensure the input file exists; create a minimal placeholder if it does not.
            if (!File.Exists(inputPath))
            {
                try
                {
                    // Create a simple email message as a placeholder.
                    using (var placeholder = new MapiMessage(
                        "organizer@example.com",
                        "attendee@example.com",
                        "Sample Meeting",
                        "This is a placeholder meeting."))
                    {
                        placeholder.Save(inputPath);
                    }
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to create placeholder MSG file: {ex.Message}");
                    return;
                }
            }

            // Load the existing MSG file.
            try
            {
                using (MapiMessage message = MapiMessage.Load(inputPath))
                {
                    // Define an RRULE string for a monthly recurrence.
                    string rrule = "FREQ=MONTHLY;INTERVAL=1;COUNT=5";

                    // Parse the RRULE into a recurrence pattern.
                    MapiCalendarRecurrencePattern recurrencePattern =
                        MapiCalendarRecurrencePatternFactory.FromString(rrule);

                    // Try to obtain the calendar part of the message.
                    MapiCalendar calendar = null;

                    // In newer Aspose.Email versions the Calendar property exists.
                    // Fallback to GetMapiCalendar() for older versions.
                    var calendarProp = typeof(MapiMessage).GetProperty("Calendar");
                    if (calendarProp != null)
                    {
                        calendar = calendarProp.GetValue(message) as MapiCalendar;
                    }
                    else
                    {
                        var getMethod = typeof(MapiMessage).GetMethod("GetMapiCalendar");
                        if (getMethod != null)
                        {
                            calendar = getMethod.Invoke(message, null) as MapiCalendar;
                        }
                    }

                    if (calendar != null)
                    {
MapiCalendarEventRecurrence eventRecurrence = new MapiCalendarEventRecurrence();
                    eventRecurrence.RecurrencePattern = recurrencePattern;
                    calendar.Recurrence = eventRecurrence;

                        // Save the updated message.
                        message.Save(outputPath);
                        Console.WriteLine($"Updated message saved to '{outputPath}'.");
                    }
                    else
                    {
                        Console.Error.WriteLine("The loaded MSG does not contain a calendar item.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error processing MSG file: {ex.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
