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
            string inputPath = "calendar.msg";
            string outputPath = "calendar.ics";

            // Verify input file exists
            if (!File.Exists(inputPath))
            {
                try
                {
                    MapiCalendar placeholderCalendar = new MapiCalendar(
                        "Placeholder Location",
                        "Placeholder Summary",
                        "Placeholder Description",
                        DateTime.Now,
                        DateTime.Now.AddHours(1));
                    if (string.IsNullOrEmpty(placeholderCalendar.Subject))
                    {
                        placeholderCalendar.Subject = "Placeholder Summary";
                    }
                    if (string.IsNullOrEmpty(placeholderCalendar.Body))
                    {
                        placeholderCalendar.Body = "Placeholder Description";
                    }
                    placeholderCalendar.Save(inputPath, MapiCalendarSaveOptions.DefaultMsg);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file '{inputPath}' does not exist.");
                return;
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(inputPath))
            {
                if (msg.SupportedType != MapiItemType.Calendar)
                {
                    Console.Error.WriteLine("The MSG file does not contain a calendar item. Creating placeholder ICS.");
                    try
                    {
                        File.WriteAllText(outputPath, "BEGIN:VCALENDAR\r\nEND:VCALENDAR");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to write placeholder ICS: {ex.Message}");
                    }
                    return;
                }

                // Convert to MapiCalendar
                using (MapiCalendar calendar = (MapiCalendar)msg.ToMapiMessageItem())
                {
                    // Change time zone to Eastern Time
                    TimeZoneInfo eastern = null;
                    try
                    {
                        eastern = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
                    }
                    catch (TimeZoneNotFoundException)
                    {
                        Console.Error.WriteLine("Eastern Time zone not found on this system.");
                    }
                    catch (InvalidTimeZoneException)
                    {
                        Console.Error.WriteLine("Eastern Time zone data is invalid.");
                    }

                    if (eastern != null)
                    {
                        MapiCalendarTimeZone tz = new MapiCalendarTimeZone(eastern);
                        calendar.StartDateTimeZone = tz;
                        calendar.EndDateTimeZone = tz;
                    }

                    // Save as ICS
                    try
                    {
                        MapiCalendarIcsSaveOptions saveOptions = new MapiCalendarIcsSaveOptions();
                        calendar.Save(outputPath, saveOptions);
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save ICS file: {ex.Message}");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
