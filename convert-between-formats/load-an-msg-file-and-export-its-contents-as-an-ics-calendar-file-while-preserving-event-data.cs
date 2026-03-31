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
                    placeholderCalendar.Save(inputPath, new MapiCalendarMsgSaveOptions());
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
                // Check if the MSG is a calendar item
                if (msg.SupportedType != MapiItemType.Calendar)
                {
                    // Create a minimal placeholder ICS file
                    try
                    {
                        File.WriteAllText(outputPath, "BEGIN:VCALENDAR\r\nEND:VCALENDAR");
                        Console.WriteLine("Input MSG is not a calendar. Placeholder ICS created.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to write placeholder ICS: {ex.Message}");
                    }
                    return;
                }

                // Convert to MapiCalendar
                MapiCalendar calendar = (MapiCalendar)msg.ToMapiMessageItem();

                // Ensure required properties are set
                if (string.IsNullOrEmpty(calendar.Subject))
                {
                    calendar.Subject = "Untitled Event";
                }
                if (string.IsNullOrEmpty(calendar.Body))
                {
                    calendar.Body = "No description.";
                }

                // Export to ICS
                calendar.Save(outputPath);
                Console.WriteLine($"Calendar exported to {outputPath}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
        }
    }
}
