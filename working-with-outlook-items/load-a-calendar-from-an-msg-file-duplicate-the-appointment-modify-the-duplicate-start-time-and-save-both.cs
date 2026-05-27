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
            string originalOutputPath = "output/original_copy.msg";
            string duplicateOutputPath = "output/duplicate.msg";

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

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDir = Path.GetDirectoryName(originalOutputPath);
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the MSG file
            using (MapiMessage originalMessage = MapiMessage.Load(inputPath))
            {
                // Check that the message contains a calendar item
                if (originalMessage.SupportedType != MapiItemType.Calendar)
                {
                    string placeholderIcsPath = Path.ChangeExtension(originalOutputPath, ".ics");
                    try
                    {
                        File.WriteAllText(placeholderIcsPath, "BEGIN:VCALENDAR\r\nEND:VCALENDAR\r\n");
                        Console.WriteLine($"Input MSG is not a calendar item. Placeholder ICS created at {placeholderIcsPath}");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Error writing placeholder ICS: {ex.Message}");
                    }
                    return;

                    Console.Error.WriteLine("The provided MSG file does not contain a calendar item.");
                    return;
                }

                // Convert to MapiCalendar
                MapiCalendar originalCalendar = (MapiCalendar)originalMessage.ToMapiMessageItem();

                // Save the original calendar to a new file (preserving original data)
                originalCalendar.Save(originalOutputPath, MapiCalendarSaveOptions.DefaultMsg);

                // Duplicate the appointment
                MapiCalendar duplicateCalendar = new MapiCalendar(
                    originalCalendar.Location,
                    originalCalendar.Subject,
                    originalCalendar.Body,
                    originalCalendar.StartDate,
                    originalCalendar.EndDate);

                // Modify the duplicate's start time (e.g., add one hour)
                duplicateCalendar.StartDate = originalCalendar.StartDate.AddHours(1);
                // Adjust end time to keep the same duration
                TimeSpan duration = originalCalendar.EndDate - originalCalendar.StartDate;
                duplicateCalendar.EndDate = duplicateCalendar.StartDate.Add(duration);

                // Save the duplicate calendar
                duplicateCalendar.Save(duplicateOutputPath, MapiCalendarSaveOptions.DefaultMsg);
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
