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
            string msgPath = "calendar.msg";
            string icsPath = "event.ics";

            // Verify input MSG file exists
            if (!File.Exists(msgPath))
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
                    placeholderCalendar.Save(msgPath, MapiCalendarSaveOptions.DefaultMsg);
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"Error creating placeholder MSG: {ex.Message}");
                    return;
                }

                Console.Error.WriteLine($"Input file not found: {msgPath}");
                return;
            }

            // Load the MSG file safely
            try
            {
                using (MapiMessage msg = MapiMessage.Load(msgPath))
                {
                    // Ensure the MSG contains a calendar item
                    if (msg.SupportedType != MapiItemType.Calendar)
                    {
                        // Create a minimal placeholder .ics file
                        try
                        {
                            string placeholderIcs = "BEGIN:VCALENDAR\r\nEND:VCALENDAR";
                            File.WriteAllText(icsPath, placeholderIcs);
                            Console.WriteLine($"Placeholder iCalendar created at: {icsPath}");
                        }
                        catch (Exception ioEx)
                        {
                            Console.Error.WriteLine($"Failed to write placeholder iCalendar: {ioEx.Message}");
                        }
                        return;
                    }

                    // Convert to MapiCalendar
                    MapiCalendar calendar = (MapiCalendar)msg.ToMapiMessageItem();

                    // Save the calendar as iCalendar (.ics) using default options
                    try
                    {
                        using (calendar)
                        {
                            MapiCalendarIcsSaveOptions saveOptions = new MapiCalendarIcsSaveOptions();
                            calendar.Save(icsPath, saveOptions);
                        }
                        Console.WriteLine($"iCalendar file saved to: {icsPath}");
                    }
                    catch (Exception saveEx)
                    {
                        Console.Error.WriteLine($"Failed to save iCalendar: {saveEx.Message}");
                    }
                }
            }
            catch (Exception loadEx)
            {
                Console.Error.WriteLine($"Failed to load MSG file: {loadEx.Message}");
            }
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Unexpected error: {ex.Message}");
        }
    }
}
