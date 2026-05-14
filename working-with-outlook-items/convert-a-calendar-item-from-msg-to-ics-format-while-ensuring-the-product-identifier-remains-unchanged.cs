using Aspose.Email;
using System;
using System.IO;
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

                Console.Error.WriteLine($"Input file not found: {inputPath}");
                return;
            }

            // Ensure output directory exists
            string outputDirectory = Path.GetDirectoryName(outputPath);
            if (!string.IsNullOrEmpty(outputDirectory) && !Directory.Exists(outputDirectory))
            {
                Directory.CreateDirectory(outputDirectory);
            }

            try
            {
                using (MapiMessage msg = MapiMessage.Load(inputPath))
                {
                    if (msg.SupportedType == MapiItemType.Calendar)
                    {
                        using (MapiCalendar calendar = (MapiCalendar)msg.ToMapiMessageItem())
                        {
                            MapiCalendarIcsSaveOptions saveOptions = new MapiCalendarIcsSaveOptions();
                            // ProductIdentifier is left unchanged (default value)
                            calendar.Save(outputPath, saveOptions);
                            Console.WriteLine("Calendar converted to ICS successfully.");
                        }
                    }
                    else
                    {
                        // Create minimal placeholder ICS file
                        using (StreamWriter writer = new StreamWriter(outputPath))
                        {
                            writer.WriteLine("BEGIN:VCALENDAR");
                            writer.WriteLine("END:VCALENDAR");
                        }
                        Console.WriteLine("Input MSG is not a calendar. Placeholder ICS created.");
                    }
                }
            }
            catch (Exception ioEx)
            {
                Console.Error.WriteLine($"IO error: {ioEx.Message}");
                return;
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
