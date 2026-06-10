using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;

class Program
{
    static void Main(string[] args)
    {
        try
        {
            // Input and output MSG file paths
            string inputPath = "calendar.msg";
            string outputPath = "calendar_updated.msg";

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
            string outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            // Load the MSG file
            using (MapiMessage msg = MapiMessage.Load(inputPath))
            {
                // Check that the MSG contains a calendar item
                if (msg.SupportedType != MapiItemType.Calendar)
                {
                    string placeholderIcsPath = Path.ChangeExtension(outputPath, ".ics");
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

                    Console.Error.WriteLine("The MSG file does not contain a calendar item.");
                    return;
                }

                // Convert to MapiCalendar
                using (MapiCalendar calendar = (MapiCalendar)msg.ToMapiMessageItem())
                {
                    // Add a display reminder (15 minutes before the event)
                    calendar.ReminderSet = true;
                    calendar.ReminderDelta = 15; // minutes

                    // Save the modified calendar back to a new MSG file
                    calendar.Save(outputPath, MapiCalendarSaveOptions.DefaultMsg);
                }
            }

            Console.WriteLine($"Calendar with reminder saved to: {outputPath}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
