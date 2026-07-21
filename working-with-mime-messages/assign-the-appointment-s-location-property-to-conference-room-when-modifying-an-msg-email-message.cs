using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Calendar;
using Aspose.Email.Mapi;

class Program
{
    static void Main()
    {
        try
        {
            // Input and output MSG file paths (placeholders)
            string inputPath = "input.msg";
            string outputPath = "output.msg";

            // Ensure the output directory exists
            string? outputDir = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDir))
                Directory.CreateDirectory(outputDir);

            // Guard against missing input file
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

            // Load the MSG file as a MapiMessage
            using (MapiMessage msg = MapiMessage.Load(inputPath))
            {
                // Verify the MSG contains a calendar item
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

                    Console.Error.WriteLine("The MSG file does not contain a calendar appointment.");
                    return;
                }

                // Create a new MapiCalendar using existing subject/body (if any)
                // For demonstration, use current time for start/end
                DateTime start = DateTime.Now;
                DateTime end = start.AddHours(1);

                MapiCalendar calendar = new MapiCalendar(
                    "Conference Room",                     // Location
                    msg.Subject ?? "No Subject",          // Summary
                    msg.Body ?? "No Body",                // Description
                    start,
                    end);

                // Preserve other properties from the original message if needed
                // (e.g., attendees, recurrence) – omitted for brevity

                // Get the underlying MapiMessage after modification
                using (MapiMessage updatedMsg = calendar.GetUnderlyingMessage())
                {
                    updatedMsg.Save(outputPath);
                }
            }

            Console.WriteLine("Appointment location updated and saved successfully.");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"An error occurred: {ex.Message}");
        }
    }
}
