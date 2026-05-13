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

            // Ensure input file exists; create a minimal placeholder if missing.
            if (!File.Exists(inputPath))
            {
                try
                {
                    using (MapiMessage placeholder = new MapiMessage("from@example.com", "to@example.com", "Placeholder", "Body"))
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

            // Load the MSG file.
            MapiMessage msg;
            try
            {
                msg = MapiMessage.Load(inputPath);
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {ex.Message}");
                return;
            }

            using (msg)
            {
                // Verify that the message contains a calendar item.
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

                    Console.Error.WriteLine("The provided MSG file does not contain a calendar item.");
                    return;
                }

                // Convert to MapiCalendar.
                MapiCalendar calendar;
                try
                {
                    calendar = (MapiCalendar)msg.ToMapiMessageItem();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Failed to convert to MapiCalendar: {ex.Message}");
                    return;
                }

                using (calendar)
                {
                    // Configure save options to keep original timestamps.
                    MapiCalendarIcsSaveOptions saveOptions = new MapiCalendarIcsSaveOptions
                    {
                        KeepOriginalDateTimeStamp = true
                    };

                    // Ensure output directory exists.
                    string outputDir = Path.GetDirectoryName(outputPath);
                    if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                    {
                        try
                        {
                            Directory.CreateDirectory(outputDir);
                        }
                        catch (Exception ex)
                        {
                            Console.Error.WriteLine($"Failed to create output directory: {ex.Message}");
                            return;
                        }
                    }

                    // Save the calendar to an ICS file with the configured options.
                    try
                    {
                        calendar.Save(outputPath, saveOptions);
                        Console.WriteLine($"Calendar saved successfully to '{outputPath}'.");
                    }
                    catch (Exception ex)
                    {
                        Console.Error.WriteLine($"Failed to save calendar: {ex.Message}");
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
