using System;
using System.IO;
using Aspose.Email;
using Aspose.Email.Mapi;
using Aspose.Email.Calendar;
using Aspose.Email.Calendar.Recurrences;

class Program
{
    static void Main()
    {
        try
        {
            string inputPath = "calendar.msg";
            string outputPath = "calendar.ics";

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

            try
            {
                using (MapiMessage msg = MapiMessage.Load(inputPath))
                {
                    if (msg.SupportedType != MapiItemType.Calendar)
                    {
                        Console.Error.WriteLine("The MSG file does not contain a calendar item.");
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

                    MapiCalendar mapiCalendar = msg.ToMapiMessageItem() as MapiCalendar;
                    if (mapiCalendar == null)
                    {
                        Console.Error.WriteLine("Failed to convert MSG to MapiCalendar.");
                        return;
                    }

                    using (mapiCalendar)
                    {
                        mapiCalendar.Save(outputPath);
                        Console.WriteLine($"Calendar saved to '{outputPath}'.");
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
