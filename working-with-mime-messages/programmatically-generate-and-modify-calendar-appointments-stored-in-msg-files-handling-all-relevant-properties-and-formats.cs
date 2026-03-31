using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Calendar;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "appointment.msg";
            string outputPath = "output/updated_appointment.msg";

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

                Console.Error.WriteLine($"Error: Input file not found – {inputPath}");
                return;
            }

            string outputDir = Path.GetDirectoryName(outputPath);
            if (!Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            using (MapiMessage msg = MapiMessage.Load(inputPath))
            {
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

                    Console.Error.WriteLine("Error: The MSG file does not contain a calendar item.");
                    return;
                }

                using (MapiCalendar calendar = (MapiCalendar)msg.ToMapiMessageItem())
                {
                    calendar.Subject = "Updated Meeting Subject";
                    calendar.Location = "Conference Room B";
                    calendar.StartDate = new DateTime(2023, 12, 1, 10, 0, 0);
                    calendar.EndDate = new DateTime(2023, 12, 1, 11, 0, 0);
                    calendar.Body = "Updated meeting description.";

                    MapiCalendarSaveOptions saveOptions = MapiCalendarSaveOptions.DefaultMsg;
                    calendar.Save(outputPath, saveOptions);
                }
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Exception: {ex.Message}");
        }
    }
}
